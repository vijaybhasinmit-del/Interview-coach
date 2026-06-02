# Interview Coach Frontend

This repository contains the frontend for the Interview Coach application, built with Vue 3 and Vite.

## Project structure

- `index.html` - application entry point
- `src/main.js` - Vue application bootstrap
- `src/App.vue` - root component
- `src/components/` - reusable Vue components
- `src/views/` - page views such as `InterviewView`, `ScorecardView`, and `SetupView`
- `src/services/interviewApi.js` - API client for interview-related requests
- `vite.config.js` - Vite configuration

## Requirements

- Node.js 18+ (or compatible version)
- npm

## Setup

```bash
npm install
```

## Development

```bash
npm run dev
```

Open the provided local URL in your browser to view the app.

## Build

```bash
npm run build
```

## Preview

```bash
npm run preview
```

## Notes

This frontend is designed to work with the Interview Coach backend API. Verify the backend URL and API integration if you modify `src/services/interviewApi.js`.
