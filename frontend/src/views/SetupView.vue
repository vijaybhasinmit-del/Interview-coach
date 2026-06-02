<template>
  <div class="setup-wrap">
    <div class="setup-card">
      <div class="brand">
        <span class="brand-icon">⚡</span>
        <span class="brand-name">PrepAI</span>
      </div>
      <h1 class="setup-title">Mock Interview Coach</h1>
      <p class="setup-sub">Paste your resume + JD and get grilled by an AI interviewer</p>

      <label class="field-label">Your Name</label>
      <input
        v-model="form.candidateName"
        class="field-input"
        placeholder="e.g. Viraj Sharma"
      />

      <label class="field-label">📄 Resume</label>
      <textarea
        v-model="form.resume"
        class="field-area"
        placeholder="Paste your resume text — experience, skills, education..."
        rows="7"
      />

      <label class="field-label">🎯 Job Description</label>
      <textarea
        v-model="form.jobDescription"
        class="field-area"
        placeholder="Paste the JD — role, responsibilities, required skills..."
        rows="7"
      />

      <button
        class="start-btn"
        :disabled="!canStart || loading"
        @click="$emit('start', form)"
      >
        <span v-if="loading" class="spinner" />
        <span v-else>Start Mock Interview →</span>
      </button>
    </div>
  </div>
</template>

<script setup>
import { reactive, computed } from 'vue'

const props = defineProps({ loading: Boolean })
const emit = defineEmits(['start'])

const form = reactive({ candidateName: '', resume: '', jobDescription: '' })
const canStart = computed(() =>
  form.candidateName.trim() && form.resume.trim() && form.jobDescription.trim()
)
</script>

<style scoped>
.setup-wrap {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 32px 16px;
}
.setup-card {
  width: 100%;
  max-width: 680px;
  background: #0f0f1c;
  border: 1px solid #1e1e35;
  border-radius: 18px;
  padding: 40px;
}
.brand {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 20px;
}
.brand-icon { font-size: 24px; }
.brand-name {
  font-size: 22px;
  font-weight: 900;
  color: #00e676;
  letter-spacing: 3px;
}
.setup-title {
  font-size: 26px;
  font-weight: 700;
  color: #fff;
  margin: 0 0 8px;
}
.setup-sub {
  color: #555;
  font-size: 14px;
  margin-bottom: 28px;
}
.field-label {
  display: block;
  font-size: 12px;
  color: #00e676;
  letter-spacing: 1px;
  text-transform: uppercase;
  margin-bottom: 8px;
}
.field-input,
.field-area {
  width: 100%;
  background: #07070f;
  border: 1px solid #1e1e35;
  border-radius: 10px;
  color: #ddd;
  padding: 13px 14px;
  font-size: 13px;
  font-family: inherit;
  margin-bottom: 20px;
  outline: none;
  box-sizing: border-box;
  transition: border-color 0.2s;
}
.field-area { resize: vertical; }
.field-input:focus,
.field-area:focus { border-color: #00e67644; }

.start-btn {
  width: 100%;
  background: #00e676;
  color: #000;
  border: none;
  border-radius: 10px;
  padding: 15px;
  font-size: 16px;
  font-weight: 800;
  cursor: pointer;
  letter-spacing: 1px;
  margin-top: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  transition: transform 0.15s, opacity 0.2s;
}
.start-btn:disabled { opacity: 0.35; cursor: not-allowed; }
.start-btn:not(:disabled):hover { transform: scale(1.01); }

.spinner {
  width: 18px; height: 18px;
  border: 2px solid #000;
  border-top-color: transparent;
  border-radius: 50%;
  animation: spin 0.7s linear infinite;
}
@keyframes spin { to { transform: rotate(360deg); } }
</style>
