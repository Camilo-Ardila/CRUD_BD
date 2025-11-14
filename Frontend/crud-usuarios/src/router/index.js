import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import DepartamentosView from '../views/DepartamentosView.vue'
import CiudadesView from '../views/CiudadesView.vue'
import UsuariosView from '../views/UsuariosView.vue'
import AccionesView from '../views/AccionesView.vue'
import LogsView from '../views/LogsView.vue'

const routes = [
  { path: '/', name: 'home', component: HomeView },
  { path: '/departamentos', name: 'departamentos', component: DepartamentosView },
  { path: '/ciudades', name: 'ciudades', component: CiudadesView },
  { path: '/usuarios', name: 'usuarios', component: UsuariosView },
  { path: '/acciones', name: 'acciones', component: AccionesView },
  { path: '/logs', name: 'logs', component: LogsView }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router