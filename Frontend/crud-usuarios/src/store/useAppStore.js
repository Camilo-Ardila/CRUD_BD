import { defineStore } from 'pinia';

const API_URL = 'http://localhost:8080/api';

export const useAppStore = defineStore('app', {
  state: () => ({
    usuarios: [],
    ciudades: [],
    departamentos: [],
    cargando: false,
    mensaje: { tipo: '', texto: '' },
  }),
  actions: {
    async cargarDatosIniciales() {
      this.cargando = true;
      try {
        await Promise.all([
          this.cargarUsuarios(),
          this.cargarCiudades(),
          this.cargarDepartamentos()
        ]);
      } catch (e) {
        this.mostrarMensaje('error', 'Error al cargar datos');
      } finally {
        this.cargando = false;
      }
    },
    async cargarUsuarios() {
      const res = await fetch(`${API_URL}/usuarios`);
      this.usuarios = res.ok ? await res.json() : [];
    },
    async cargarCiudades() {
      const res = await fetch(`${API_URL}/ciudades`);
      this.ciudades = res.ok ? await res.json() : [];
    },
    async cargarDepartamentos() {
      const res = await fetch(`${API_URL}/departamentos`);
      this.departamentos = res.ok ? await res.json() : [];
    },
    mostrarMensaje(tipo, texto) {
      this.mensaje = { tipo, texto };
      setTimeout(() => (this.mensaje = { tipo: '', texto: '' }), 4000);
    },
  },
});
