<template>
  <div class="sc-wrap">
    <div class="sc-card">
      <div class="score-hero">
        <span class="score-num">{{ card.overall }}</span>
        <span class="score-denom">/10</span>
      </div>
      <p class="verdict">{{ card.verdict }}</p>

      <!-- Category bars -->
      <div class="cats">
        <div
          v-for="(score, cat) in card.categories"
          :key="cat"
          class="cat-row"
        >
          <span class="cat-name">{{ cat }}</span>
          <div class="cat-track">
            <div
              class="cat-fill"
              :style="{
                width: score * 10 + '%',
                background: score >= 7 ? '#00e676' : score >= 5 ? '#ffb300' : '#ff5252'
              }"
            />
          </div>
          <span class="cat-score">{{ score }}/10</span>
        </div>
      </div>

      <!-- Strengths + Improvements -->
      <div class="swot-row">
        <div class="swot-card">
          <div class="swot-title green">✅ Strengths</div>
          <div v-for="(s, i) in card.strengths" :key="i" class="swot-item">• {{ s }}</div>
        </div>
        <div class="swot-card">
          <div class="swot-title orange">⚠️ Improve</div>
          <div v-for="(s, i) in card.improvements" :key="i" class="swot-item">• {{ s }}</div>
        </div>
      </div>

      <button class="retry-btn" @click="$emit('restart')">🔄 Practice Again</button>
    </div>
  </div>
</template>

<script setup>
defineProps({ card: Object })
defineEmits(['restart'])
</script>

<style scoped>
.sc-wrap {
  min-height: 100vh;
  display: flex;
  align-items: flex-start;
  justify-content: center;
  padding: 40px 16px;
  overflow-y: auto;
}
.sc-card {
  width: 100%;
  max-width: 680px;
  display: flex;
  flex-direction: column;
  gap: 28px;
  animation: fadeUp 0.4s ease;
}
.score-hero { display: flex; align-items: baseline; gap: 4px; justify-content: center; }
.score-num  { font-size: 96px; font-weight: 900; color: #00e676; line-height: 1; }
.score-denom{ font-size: 32px; color: #333; }
.verdict    { text-align: center; font-size: 17px; color: #aaa; margin: -16px 0 0; }

/* Cats */
.cats { display: flex; flex-direction: column; gap: 12px; }
.cat-row {
  background: #0f0f1c; border: 1px solid #1e1e35;
  border-radius: 10px; padding: 12px 16px;
  display: flex; align-items: center; gap: 12px;
}
.cat-name  { font-size: 13px; color: #aaa; width: 165px; flex-shrink: 0; }
.cat-track { flex: 1; height: 6px; background: #1e1e2e; border-radius: 99px; }
.cat-fill  { height: 100%; border-radius: 99px; transition: width 1s ease; }
.cat-score { font-size: 13px; color: #eee; width: 36px; text-align: right; }

/* SWOT */
.swot-row { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
.swot-card {
  background: #0f0f1c; border: 1px solid #1e1e35;
  border-radius: 10px; padding: 16px;
}
.swot-title { font-size: 12px; font-weight: 800; letter-spacing: 1px; margin-bottom: 10px; }
.swot-title.green  { color: #00e676; }
.swot-title.orange { color: #ff8a65; }
.swot-item { font-size: 13px; color: #999; line-height: 1.9; }

.retry-btn {
  width: 100%;
  background: #00e676; color: #000; border: none;
  border-radius: 10px; padding: 15px;
  font-size: 16px; font-weight: 800; cursor: pointer;
  letter-spacing: 1px; transition: transform 0.15s;
}
.retry-btn:hover { transform: scale(1.01); }

@keyframes fadeUp {
  from { opacity: 0; transform: translateY(14px); }
  to   { opacity: 1; transform: translateY(0); }
}
</style>
