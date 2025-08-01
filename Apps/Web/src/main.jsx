import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'
import './index.css'
import HomeAdmin from './views/HomeAdmin.jsx'
import HomeContent from './views/HomeContent.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <HomeContent />
  </StrictMode>,
)
