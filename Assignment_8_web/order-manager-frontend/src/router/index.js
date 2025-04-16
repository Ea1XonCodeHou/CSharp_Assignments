import { createRouter, createWebHistory } from 'vue-router';
import Login from '../views/Login.vue';
import OrderList from '../views/OrderList.vue';
import Register from '../views/Register.vue';

// 定义需要登录权限的路由
const authRoutes = ['/home', '/dashboard', '/ai-assistant', '/settings', '/messages'];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/login'
    },
    {
      path: '/login',
      name: 'login',
      component: Login
    },
    {
      path: '/register',
      name: 'register',
      component: Register
    },
    {
      path: '/orders',
      name: 'orders',
      component: OrderList,
      meta: { requiresAuth: true }
    }
  ]
})

// 路由守卫
router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')
  
  if (to.meta.requiresAuth && !token) {
    next('/login')
  } else {
    next()
  }
})

export default router
