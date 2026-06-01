<template>
  <div class="iv-wrap">
    <!-- Progress bar -->
    <div class="progress-bar">
      <span class="progress-label">Question {{ questionNumber }} / {{ totalQuestions }}</span>
      <div class="progress-track">
        <div class="progress-fill" :style="{ width: fillPct + '%' }" />
      </div>
      <button class="abandon-btn" @click="$emit('abandon')">✕ Quit</button>
    </div>

    <!-- Chat -->
    <div class="chat-box" ref="chatBox">
      <template v-for="(msg, i) in messages" :key="i">
        <!-- AI message -->
        <div v-if="msg.role === 'ai'" class="row-ai">
          <div class="avatar-ai">AI</div>
          <div class="bubble-ai">
            <template v-for="(line, li) in msg.lines" :key="li">
              <p
                :class="{
                  'line-good': line.startsWith('✅'),
                  'line-warn': line.startsWith('⚠️'),
                  'line-q':    !line.startsWith('✅') && !line.startsWith('⚠️')
                }"
              >{{ line }}</p>
            </template>
          </div>
        </div>
        <!-- User message -->
        <div v-else class="row-user">
          <div class="bubble-user">{{ msg.text }}</div>
          <div class="avatar-user">You</div>
        </div>
      </template>

      <!-- Typing indicator -->
      <div v-if="loading" class="row-ai">
        <div class="avatar-ai">AI</div>
        <div class="bubble-ai typing">
          <span /><span /><span />
        </div>
      </div>

      <div ref="bottom" />
    </div>

    <!-- Input -->
    <div class="input-row">
      <textarea
        v-model="draft"
        class="chat-input"
        placeholder="Type your answer… (Enter to send, Shift+Enter for newline)"
        rows="3"
        @keydown.enter.exact.prevent="send"
        :disabled="loading"
      />
      <button class="send-btn" :disabled="loading || !draft.trim()" @click="send">
        Send
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch, nextTick } from 'vue'

const props = defineProps({
  firstQuestion: String,
  questionNumber: Number,
  totalQuestions: Number,
  loading: Boolean
})
const emit = defineEmits(['answer', 'abandon'])

const draft = ref('')
const chatBox = ref(null)
const bottom = ref(null)

// Build messages list — seed with first AI question
const messages = ref([
  { role: 'ai', lines: (props.firstQuestion || '').split('\n').filter(Boolean) }
])

const fillPct = computed(() =>
  Math.min(((props.questionNumber - 1) / props.totalQuestions) * 100, 100)
)

// Called by parent to push AI response after answer
function pushAi(text) {
  messages.value.push({
    role: 'ai',
    lines: text.split('\n').filter(Boolean)
  })
  scrollBottom()
}

function pushUser(text) {
  messages.value.push({ role: 'user', text })
  scrollBottom()
}

function send() {
  if (!draft.value.trim() || props.loading) return
  const ans = draft.value.trim()
  pushUser(ans)
  draft.value = ''
  emit('answer', ans)
}

async function scrollBottom() {
  await nextTick()
  bottom.value?.scrollIntoView({ behavior: 'smooth' })
}

defineExpose({ pushAi, pushUser })
</script>

<style scoped>
.iv-wrap {
  display: flex;
  flex-direction: column;
  height: 100vh;
  max-width: 820px;
  margin: 0 auto;
  padding: 0 16px 16px;
}

/* Progress */
.progress-bar {
  display: flex;
  align-items: center;
  gap: 14px;
  padding: 14px 0;
  border-bottom: 1px solid #111;
  flex-shrink: 0;
}
.progress-label { font-size: 12px; color: #555; white-space: nowrap; }
.progress-track { flex: 1; height: 4px; background: #1e1e2e; border-radius: 99px; }
.progress-fill  { height: 100%; background: #00e676; border-radius: 99px; transition: width 0.5s; }
.abandon-btn {
  background: transparent;
  border: 1px solid #333;
  color: #666;
  padding: 5px 12px;
  border-radius: 6px;
  cursor: pointer;
  font-size: 12px;
  transition: color 0.2s;
}
.abandon-btn:hover { color: #ff5252; border-color: #ff5252; }

/* Chat */
.chat-box {
  flex: 1;
  overflow-y: auto;
  padding: 20px 0 12px;
  display: flex;
  flex-direction: column;
  gap: 18px;
}
.chat-box::-webkit-scrollbar { width: 4px; }
.chat-box::-webkit-scrollbar-thumb { background: #1e1e35; border-radius: 4px; }

.row-ai   { display: flex; gap: 10px; align-items: flex-start; animation: msgIn 0.25s ease; }
.row-user { display: flex; gap: 10px; align-items: flex-start; justify-content: flex-end; animation: msgIn 0.25s ease; }

.avatar-ai {
  background: #00e676; color: #000;
  font-size: 9px; font-weight: 800;
  border-radius: 50%; width: 30px; height: 30px;
  display: flex; align-items: center; justify-content: center;
  flex-shrink: 0;
}
.avatar-user {
  background: #1e1e35; color: #888;
  font-size: 9px; border-radius: 50%;
  width: 30px; height: 30px;
  display: flex; align-items: center; justify-content: center;
  flex-shrink: 0;
}
.bubble-ai {
  background: #0f0f22;
  border: 1px solid #1e1e35;
  border-radius: 4px 16px 16px 16px;
  padding: 14px 16px;
  max-width: 78%;
  font-size: 14px;
  line-height: 1.7;
}
.bubble-user {
  background: #001a0e;
  border: 1px solid #00e67622;
  border-radius: 16px 4px 16px 16px;
  padding: 12px 16px;
  max-width: 78%;
  font-size: 14px;
  color: #b3ffda;
  line-height: 1.7;
  white-space: pre-wrap;
}
.bubble-ai p { margin: 2px 0; }
.line-good { color: #00e676; }
.line-warn { color: #ffb300; }
.line-q    { color: #e0e0f0; }

/* Typing */
.typing { display: flex; gap: 5px; align-items: center; padding: 16px; }
.typing span {
  display: inline-block; width: 7px; height: 7px;
  border-radius: 50%; background: #00e676;
  animation: bounce 1.2s infinite;
}
.typing span:nth-child(2) { animation-delay: 0.2s; }
.typing span:nth-child(3) { animation-delay: 0.4s; }

/* Input */
.input-row { display: flex; gap: 10px; flex-shrink: 0; padding-top: 8px; border-top: 1px solid #111; }
.chat-input {
  flex: 1;
  background: #0f0f1c; border: 1px solid #1e1e35;
  border-radius: 10px; color: #ddd;
  padding: 12px; font-size: 14px; font-family: inherit;
  resize: none; outline: none; transition: border-color 0.2s;
}
.chat-input:focus { border-color: #00e67644; }
.send-btn {
  background: #00e676; color: #000; border: none;
  border-radius: 10px; padding: 0 22px;
  font-weight: 800; cursor: pointer; font-size: 14px;
  transition: transform 0.1s;
}
.send-btn:disabled { opacity: 0.35; cursor: not-allowed; }
.send-btn:not(:disabled):hover { transform: scale(1.03); }

@keyframes msgIn {
  from { opacity: 0; transform: translateY(8px); }
  to   { opacity: 1; transform: translateY(0); }
}
@keyframes bounce {
  0%,80%,100% { transform: translateY(0); }
  40%          { transform: translateY(-6px); }
}
</style>
