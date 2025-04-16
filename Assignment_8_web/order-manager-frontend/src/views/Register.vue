<template>
  <div class="register-container">
    <div class="register-box">
      <h2>注册账号</h2>
      <div class="form-group">
        <input type="text" v-model="username" placeholder="用户名" />
      </div>
      <div class="form-group">
        <input type="password" v-model="password" placeholder="密码" />
      </div>
      <div class="form-group">
        <input type="password" v-model="confirmPassword" placeholder="确认密码" />
      </div>
      <div class="form-group">
        <button @click="handleRegister" :disabled="loading">
          {{ loading ? '注册中...' : '注册' }}
        </button>
      </div>
      <div class="form-group">
        <router-link to="/login">已有账号？立即登录</router-link>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

export default {
  name: 'Register',
  setup() {
    const router = useRouter()
    const username = ref('')
    const password = ref('')
    const confirmPassword = ref('')
    const loading = ref(false)

    const handleRegister = async () => {
      if (!username.value || !password.value || !confirmPassword.value) {
        alert('请填写所有字段')
        return
      }

      if (password.value !== confirmPassword.value) {
        alert('两次输入的密码不一致')
        return
      }

      loading.value = true
      try {
        const response = await axios.post('/api/user/register', {
          username: username.value,
          password: password.value
        })

        if (response.data.code === 200) {
          alert('注册成功')
          router.push('/login')
        } else {
          alert(response.data.message || '注册失败')
        }
      } catch (error) {
        alert('注册失败：' + (error.response?.data?.message || error.message))
      } finally {
        loading.value = false
      }
    }

    return {
      username,
      password,
      confirmPassword,
      loading,
      handleRegister
    }
  }
}
</script>

<style scoped>
.register-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background-color: #f5f5f5;
}

.register-box {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 400px;
}

h2 {
  text-align: center;
  color: #333;
  margin-bottom: 2rem;
}

.form-group {
  margin-bottom: 1rem;
}

input {
  width: 100%;
  padding: 0.8rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
}

button {
  width: 100%;
  padding: 0.8rem;
  background-color: #4CAF50;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 1rem;
  cursor: pointer;
}

button:disabled {
  background-color: #cccccc;
  cursor: not-allowed;
}

a {
  color: #4CAF50;
  text-decoration: none;
  display: block;
  text-align: center;
}

a:hover {
  text-decoration: underline;
}
</style> 