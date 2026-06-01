<template>
  <div id="app">
    <!-- SETUP -->
    <SetupView
      v-if="stage === 'setup'"
      :loading="loading"
      @start="handleStart"
    />

    <!-- INTERVIEW -->
    <InterviewView
      v-else-if="stage === 'interview'"
      ref="interviewRef"
      :first-question="session.firstQuestion"
      :question-number="session.questionNumber"
      :total-questions="session.totalQuestions"
      :loading="loading"
      @answer="handleAnswer"
      @abandon="reset"
    />

    <!-- SCORECARD -->
    <ScorecardView
      v-else-if="stage === 'scorecard'"
      :card="scorecard"
      @restart="reset"
    />

    <!-- Error toast -->
    <Transition name="toast">
      <div v-if="error" class="error-toast">⚠️ {{ error }}</div>
    </Transition>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import SetupView     from './views/SetupView.vue'
import InterviewView from './views/InterviewView.vue'
import ScorecardView from './views/ScorecardView.vue'
import { startSession, submitAnswer, abandonSession } from './services/interviewApi.js'

const stage       = ref('setup')   // 'setup' | 'interview' | 'scorecard'
const loading     = ref(false)
const error       = ref('')
const scorecard   = ref(null)
const interviewRef = ref(null)

const session = reactive({
  sessionId:     '',
  firstQuestion: '',
  questionNumber: 1,
  totalQuestions: 5
})

// ── Start ──────────────────────────────────────────────────
async function handleStart(form) {
  loading.value = true
  error.value   = ''
  try {
    const data = await startSession(form)
    session.sessionId     = data.sessionId
    session.firstQuestion = data.firstQuestion
    session.totalQuestions = data.totalQuestions
    session.questionNumber = 1
    stage.value = 'interview'
  } catch (e) {
    error.value = 'Could not connect to server. Is the backend running?'
    setTimeout(() => (error.value = ''), 4000)
  } finally {
    loading.value = false
  }
}

// ── Answer ─────────────────────────────────────────────────
async function handleAnswer(answer) {
  loading.value = true
  try {
    const data = await submitAnswer({ sessionId: session.sessionId, answer })
    session.questionNumber = data.questionNumber

    const aiText = data.isComplete
      ? data.feedback
      : [data.feedback, data.nextQuestion].filter(Boolean).join('\n\n')

    interviewRef.value?.pushAi(aiText)

    if (data.isComplete) {
      scorecard.value = data.scoreCard
      setTimeout(() => { stage.value = 'scorecard' }, 1800)
    }
  } catch (e) {
    error.value = 'Server error — please try again.'
    setTimeout(() => (error.value = ''), 4000)
  } finally {
    loading.value = false
  }
}

// ── Reset ──────────────────────────────────────────────────
async function reset() {
  if (session.sessionId) abandonSession(session.sessionId)
  stage.value    = 'setup'
  scorecard.value = null
  session.sessionId = ''
}
</script>

<style>
*, *::before, *::after { box-sizing: border-box; }

body {
  margin: 0;
  background: #0a0a0f;
  color: #e0e0f0;
  font-family: 'Courier New', Courier, monospace;
  -webkit-font-smoothing: antialiased;
}

#app { min-height: 100vh; }

/* Error toast */
.error-toast {
  position: fixed;
  bottom: 24px; left: 50%;
  transform: translateX(-50%);
  background: #1a0505;
  border: 1px solid #ff525244;
  color: #ff8a80;
  padding: 12px 22px;
  border-radius: 10px;
  font-size: 13px;
  z-index: 999;
}
.toast-enter-active, .toast-leave-active { transition: opacity 0.3s, transform 0.3s; }
.toast-enter-from, .toast-leave-to { opacity: 0; transform: translateX(-50%) translateY(10px); }
</style>
