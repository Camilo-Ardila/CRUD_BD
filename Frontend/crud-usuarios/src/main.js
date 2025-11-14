// src/main.js
import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'

// IMPORTS CON TUS NOMBRES (View al final)
import HomeView from './views/HomeView.vue'
import DepartamentosView from './views/DepartamentosView.vue'
import CiudadesView from './views/CiudadesView.vue'
import UsuariosView from './views/UsuariosView.vue'
import AccionesView from './views/AccionesView.vue'
import LogsView from './views/LogsView.vue'

import './index.css'
import { Logs } from 'lucide-vue-next'

const routes = [
  { path: '/', component: HomeView },
  { path: '/departamentos', component: DepartamentosView },
  { path: '/ciudades', component: CiudadesView },
  { path: '/usuarios', component: UsuariosView },
  { path: '/acciones', component : AccionesView},
  { path: '/logs', component : LogsView}
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

createApp(App).use(router).mount('#app')