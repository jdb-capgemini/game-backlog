import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),

  routes: [
    {
      path: '/',
      name: 'dashboard',
      component: () => import('@/views/DashboardView.vue'),
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/LoginView.vue'),
    },
    {
      path: '/profile',
      name: 'profile',
      component: () => import('@/views/ProfileView.vue'),
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/backlog',
      name: 'backlog',
      component: () => import('@/views/BacklogView.vue'),
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/backlog/add',
      name: 'add-game',
      component: () => import('@/views/AddGameView.vue'),
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/backlog/:id',
      name: 'backlog-details',
      component: () => import('@/views/BacklogDetailsView.vue'),
      props: (route) => ({
        id: Number(route.params.id),
      }),
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/backlog/:id/edit',
      name: 'backlog-edit',
      component: () => import('@/views/BacklogEditView.vue'),
      props: (route) => ({
        id: Number(route.params.id),
      }),
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'not-found',
      component: () => import('@/views/NotFoundView.vue'),
    },
  ],

  scrollBehavior() {
    return { top: 0 }
  },
})

router.beforeEach(async (to, from, next) => {
  const authStore = useAuthStore()

  // Initialize auth if not already done
  if (!authStore.initialized) {
    await authStore.initialize()
  }

  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth)

  if (requiresAuth && !authStore.isAuthenticated) {
    // Redirect to login if route requires auth and user is not authenticated
    next({ name: 'login', query: { returnUrl: to.fullPath } })
  } else {
    next()
  }
})

export default router
