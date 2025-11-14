<!-- src/views/UsuariosView.vue -->
<template>
  <div class="container">
    <h2>Usuarios</h2>

    <!-- FORMULARIO DE CREACIÓN -->
    <form @submit.prevent="crearUsuario" class="form">
      <input v-model="nuevo.cc" placeholder="Cédula (8 o 10 dígitos)" type="number" required />
      <input v-model="nuevo.nombre" placeholder="Nombre" required />
      <input v-model="nuevo.fechaNacimiento" type="date" required />
      <select v-model="nuevo.idCiudad" required>
        <option value="">Selecciona ciudad</option>
        <option v-for="c in ciudades" :key="c.idCiudad" :value="c.idCiudad">
          {{ c.nombreCiudad }} ({{ c.departamento }})
        </option>
      </select>
      <button type="submit">Crear</button>
    </form>

    <!-- FILTROS DE BÚSQUEDA -->
    <div class="filtros">
      <h3>Filtros de Búsqueda</h3>
      
      <!-- FILTRO POR NOMBRE -->
      <div class="filtro-grupo">
        <label>Buscar por nombre:</label>
        <div class="filtro-inputs">
          <input 
            v-model="filtros.nombre" 
            placeholder="Ingresa el nombre"
          />
          <button @click="buscarPorNombre" class="buscar">Buscar</button>
        </div>
      </div>

      <div class="separador">O</div>

      <!-- FILTRO POR RANGO DE FECHAS -->
      <div class="filtro-grupo">
        <label>Buscar por rango de fecha de nacimiento:</label>
        <div class="filtro-inputs">
          <input 
            v-model="filtros.fechaDesde" 
            type="date" 
            placeholder="Fecha desde"
          />
          <input 
            v-model="filtros.fechaHasta" 
            type="date" 
            placeholder="Fecha hasta"
          />
          <button @click="buscarPorFecha" class="buscar">Buscar</button>
        </div>
      </div>

      <button @click="limpiarFiltros" class="limpiar">Limpiar Filtros</button>
    </div>

    <div v-if="error" class="error">{{ error }}</div>

    <table class="table">
      <thead>
        <tr>
          <th>Cédula</th>
          <th>Nombre</th>
          <th>Fecha Nac.</th>
          <th>Ciudad</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="usuarios.length === 0">
          <td colspan="5" style="text-align: center">No se encontraron usuarios</td>
        </tr>
        <tr v-for="u in usuarios" :key="u.cc">
          <td>{{ u.cc }}</td>
          <td>
            <input v-if="editando === u.cc" v-model="u.nombre" />
            <span v-else>{{ u.nombre }}</span>
          </td>
          <td>
            <input v-if="editando === u.cc" v-model="u.fechaNacimiento" type="date" />
            <span v-else>{{ formatearFecha(u.fechaNacimiento) }}</span>
          </td>
          <td>
            <select v-if="editando === u.cc" v-model="u.idCiudad">
              <option v-for="c in ciudades" :key="c.idCiudad" :value="c.idCiudad">
                {{ c.nombreCiudad }}
              </option>
            </select>
            <span v-else>{{ u.nombreCiudad }}</span>
          </td>
          <td>
            <button v-if="editando === u.cc" @click="actualizar(u)" class="save">Guardar</button>
            <button v-else @click="editando = u.cc" class="edit">Editar</button>
            <button @click="eliminar(u.cc)" class="delete">Eliminar</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/services/api'

const usuarios = ref([])
const ciudades = ref([])
const nuevo = ref({
  cc: '',
  nombre: '',
  fechaNacimiento: '',
  idCiudad: ''
})
const filtros = ref({
  nombre: '',
  fechaDesde: '',
  fechaHasta: ''
})
const editando = ref(null)
const error = ref('')

onMounted(async () => {
  await cargarTodo()
})

const cargarTodo = async () => {
  try {
    const resC = await api.get('/ciudades')
    ciudades.value = resC.data
    await cargarUsuarios()
    error.value = ''
  } catch (e) {
    error.value = e.response?.data?.message || 'Error al cargar datos'
    console.error('Error cargando datos:', e)
  }
}

const cargarUsuarios = async (params = {}) => {
  try {
    const resU = await api.get('/usuarios', { params })
    usuarios.value = resU.data
  } catch (e) {
    error.value = e.response?.data?.message || 'Error al cargar usuarios'
  }
}

const buscarPorNombre = async () => {
  if (!filtros.value.nombre.trim()) {
    error.value = 'Por favor ingresa un nombre para buscar'
    return
  }
  
  // Limpiar filtros de fecha
  filtros.value.fechaDesde = ''
  filtros.value.fechaHasta = ''
  
  await cargarUsuarios({ nombre: filtros.value.nombre })
  error.value = ''
}

const buscarPorFecha = async () => {
  if (!filtros.value.fechaDesde && !filtros.value.fechaHasta) {
    error.value = 'Por favor selecciona al menos una fecha'
    return
  }
  
  // Limpiar filtro de nombre
  filtros.value.nombre = ''
  
  const params = {}
  if (filtros.value.fechaDesde) params.fechaDesde = filtros.value.fechaDesde
  if (filtros.value.fechaHasta) params.fechaHasta = filtros.value.fechaHasta
  
  await cargarUsuarios(params)
  error.value = ''
}

const limpiarFiltros = async () => {
  filtros.value = {
    nombre: '',
    fechaDesde: '',
    fechaHasta: ''
  }
  await cargarUsuarios()
  error.value = ''
}

const formatearFecha = (fecha) => {
  if (!fecha) return ''
  const date = new Date(fecha)
  return date.toLocaleDateString('es-ES', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  })
}

const crearUsuario = async () => {
  try {
    await api.post('/usuarios', nuevo.value)
    nuevo.value = { cc: '', nombre: '', fechaNacimiento: '', idCiudad: '' }
    await cargarUsuarios()
    error.value = ''
  } catch (e) {
    error.value = e.response?.data?.message || 'Error al crear'
  }
}

const actualizar = async (u) => {
  try {
    await api.put(`/usuarios/${u.cc}`, {
      cc: u.cc,
      nombre: u.nombre,
      fechaNacimiento: u.fechaNacimiento,
      idCiudad: u.idCiudad
    })
    editando.value = null
    await cargarUsuarios()
    error.value = ''
  } catch (e) {
    error.value = e.response?.data?.message || 'Error al actualizar'
  }
}

const eliminar = async (cc) => {
  if (!confirm('¿Eliminar usuario?')) return
  try {
    await api.delete(`/usuarios/${cc}`)
    await cargarUsuarios()
    error.value = ''
  } catch (e) {
    error.value = e.response?.data?.message || 'Error al eliminar'
  }
}
</script>

<style scoped>
.container { 
  padding: 2rem; 
  max-width: 1200px; 
  margin: 0 auto; 
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}
h2 { 
  color: #2c3e50; 
  text-align: center; 
  margin-bottom: 1.5rem;
}
.form { 
  display: flex; 
  gap: 0.75rem; 
  margin-bottom: 1.5rem; 
  flex-wrap: wrap; 
  background: #f8f9fa;
  padding: 1rem;
  border-radius: 8px;
}

/* ESTILOS PARA FILTROS */
.filtros {
  background: #e8f5e9;
  padding: 1.5rem;
  border-radius: 8px;
  margin-bottom: 1.5rem;
  border: 2px solid #4caf50;
}
.filtros h3 {
  margin: 0 0 1rem 0;
  color: #2e7d32;
  font-size: 1.2rem;
  text-align: center;
}
.filtro-grupo {
  margin-bottom: 1rem;
  background: white;
  padding: 1rem;
  border-radius: 6px;
}
.filtro-grupo label {
  display: block;
  font-weight: bold;
  color: #2e7d32;
  margin-bottom: 0.5rem;
}
.filtro-inputs {
  display: flex;
  gap: 0.5rem;
  flex-wrap: wrap;
}
.separador {
  text-align: center;
  font-weight: bold;
  color: #2e7d32;
  font-size: 1.1rem;
  margin: 1rem 0;
}
.buscar {
  background: #4caf50 !important;
  min-width: 100px;
}
.buscar:hover {
  background: #45a049 !important;
}
.limpiar {
  background: #ff9800 !important;
  width: 100%;
  margin-top: 0.5rem;
}
.limpiar:hover {
  background: #f57c00 !important;
}

input, select, button { 
  padding: 0.6rem; 
  font-size: 1rem; 
  border: 1px solid #ddd;
  border-radius: 4px;
}
button { 
  background: #27ae60; 
  color: white; 
  border: none; 
  cursor: pointer;
  font-weight: bold;
}
button:hover { 
  background: #219a52; 
}
.edit {
  background: #3498db;
}
.edit:hover {
  background: #2980b9;
}
.save {
  background: #27ae60;
}
.save:hover {
  background: #219a52;
}
.delete { 
  background: #e74c3c; 
  margin-left: 0.5rem; 
}
.delete:hover { 
  background: #c0392b; 
}
.table { 
  width: 100%; 
  border-collapse: collapse; 
  margin-top: 1rem; 
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}
th, td { 
  border: 1px solid #ddd; 
  padding: 0.75rem; 
  text-align: left; 
}
th { 
  background: #34495e; 
  color: white;
  font-weight: 600;
}
tr:nth-child(even) { 
  background: #f9f9f9; 
}
.error { 
  color: #e74c3c; 
  margin: 1rem 0; 
  font-weight: bold; 
  text-align: center;
  background: #fadad7;
  padding: 0.75rem;
  border-radius: 4px;
}
</style>