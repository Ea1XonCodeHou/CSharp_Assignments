<template>
  <div class="order-list-container">
    <div class="header">
      <h1>订单管理</h1>
      <button @click="showCreateOrderModal = true" class="create-btn">创建订单</button>
    </div>

    <!-- 订单列表 -->
    <div class="order-list">
      <div v-if="loading" class="loading">加载中...</div>
      <div v-else-if="orders.length === 0" class="empty-state">
        暂无订单
      </div>
      <div v-else class="orders">
        <div v-for="order in orders" :key="order.id" class="order-card">
          <div class="order-header">
            <span class="order-number">订单号: {{ order.orderNumber }}</span>
            <span :class="['order-status', order.status]">{{ getStatusText(order.status) }}</span>
          </div>
          <div class="order-info">
            <p>客户名称: {{ order.customerName }}</p>
            <p>总金额: ¥{{ order.totalAmount }}</p>
            <p>创建时间: {{ formatDate(order.createdAt) }}</p>
          </div>
          <div class="order-actions">
            <button @click="viewOrderDetails(order)" class="action-btn view">查看</button>
            <button @click="updateOrderStatus(order)" class="action-btn edit">更新状态</button>
            <button @click="deleteOrder(order.id)" class="action-btn delete">删除</button>
          </div>
        </div>
      </div>
    </div>

    <!-- 创建订单模态框 -->
    <div v-if="showCreateOrderModal" class="modal">
      <div class="modal-content">
        <h2>创建新订单</h2>
        <div class="form-group">
          <label>客户名称</label>
          <input v-model="newOrder.customerName" type="text" placeholder="请输入客户名称">
        </div>
        <div class="form-group">
          <label>商品列表</label>
          <div v-for="(item, index) in newOrder.items" :key="index" class="order-item">
            <input v-model="item.productName" type="text" placeholder="商品名称">
            <input v-model.number="item.quantity" type="number" placeholder="数量">
            <input v-model.number="item.unitPrice" type="number" placeholder="单价">
            <button @click="removeOrderItem(index)" class="remove-btn">删除</button>
          </div>
          <button @click="addOrderItem" class="add-btn">添加商品</button>
        </div>
        <div class="modal-actions">
          <button @click="createOrder" :disabled="creating">创建</button>
          <button @click="showCreateOrderModal = false">取消</button>
        </div>
      </div>
    </div>

    <!-- 订单详情模态框 -->
    <div v-if="selectedOrder" class="modal">
      <div class="modal-content">
        <h2>订单详情</h2>
        <div class="order-details">
          <p><strong>订单号:</strong> {{ selectedOrder.orderNumber }}</p>
          <p><strong>客户名称:</strong> {{ selectedOrder.customerName }}</p>
          <p><strong>总金额:</strong> ¥{{ selectedOrder.totalAmount }}</p>
          <p><strong>状态:</strong> {{ getStatusText(selectedOrder.status) }}</p>
          <p><strong>创建时间:</strong> {{ formatDate(selectedOrder.createdAt) }}</p>
          <h3>商品列表</h3>
          <div class="items-list">
            <div v-for="item in selectedOrder.items" :key="item.id" class="item">
              <span>{{ item.productName }}</span>
              <span>{{ item.quantity }} x ¥{{ item.unitPrice }}</span>
            </div>
          </div>
        </div>
        <div class="modal-actions">
          <button @click="selectedOrder = null">关闭</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import { onMounted, ref } from 'vue'

export default {
  name: 'OrderList',
  setup() {
    const orders = ref([])
    const loading = ref(false)
    const showCreateOrderModal = ref(false)
    const selectedOrder = ref(null)
    const creating = ref(false)
    const newOrder = ref({
      customerName: '',
      items: [{ productName: '', quantity: 1, unitPrice: 0 }]
    })

    // 获取订单列表
    const fetchOrders = async () => {
      loading.value = true
      try {
        const response = await axios.get('/api/order/user')
        orders.value = response.data
      } catch (error) {
        alert('获取订单列表失败：' + error.message)
      } finally {
        loading.value = false
      }
    }

    // 创建订单
    const createOrder = async () => {
      if (!newOrder.value.customerName || newOrder.value.items.length === 0) {
        alert('请填写完整订单信息')
        return
      }

      creating.value = true
      try {
        await axios.post('/api/order', newOrder.value)
        showCreateOrderModal.value = false
        fetchOrders()
        // 重置表单
        newOrder.value = {
          customerName: '',
          items: [{ productName: '', quantity: 1, unitPrice: 0 }]
        }
      } catch (error) {
        alert('创建订单失败：' + error.message)
      } finally {
        creating.value = false
      }
    }

    // 删除订单
    const deleteOrder = async (orderId) => {
      if (!confirm('确定要删除这个订单吗？')) return

      try {
        await axios.delete(`/api/order/${orderId}`)
        fetchOrders()
      } catch (error) {
        alert('删除订单失败：' + error.message)
      }
    }

    // 更新订单状态
    const updateOrderStatus = async (order) => {
      const newStatus = prompt('请输入新状态 (pending/processing/completed/cancelled):', order.status)
      if (!newStatus) return

      try {
        await axios.put(`/api/order/${order.id}/status`, newStatus)
        fetchOrders()
      } catch (error) {
        alert('更新订单状态失败：' + error.message)
      }
    }

    // 查看订单详情
    const viewOrderDetails = async (order) => {
      try {
        const response = await axios.get(`/api/order/${order.id}`)
        selectedOrder.value = response.data
      } catch (error) {
        alert('获取订单详情失败：' + error.message)
      }
    }

    // 添加订单商品
    const addOrderItem = () => {
      newOrder.value.items.push({ productName: '', quantity: 1, unitPrice: 0 })
    }

    // 删除订单商品
    const removeOrderItem = (index) => {
      newOrder.value.items.splice(index, 1)
    }

    // 格式化日期
    const formatDate = (dateString) => {
      return new Date(dateString).toLocaleString()
    }

    // 获取状态文本
    const getStatusText = (status) => {
      const statusMap = {
        pending: '待处理',
        processing: '处理中',
        completed: '已完成',
        cancelled: '已取消'
      }
      return statusMap[status] || status
    }

    onMounted(() => {
      fetchOrders()
    })

    return {
      orders,
      loading,
      showCreateOrderModal,
      selectedOrder,
      creating,
      newOrder,
      createOrder,
      deleteOrder,
      updateOrderStatus,
      viewOrderDetails,
      addOrderItem,
      removeOrderItem,
      formatDate,
      getStatusText
    }
  }
}
</script>

<style scoped>
.order-list-container {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.create-btn {
  background-color: #4CAF50;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
}

.order-list {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1);
  padding: 20px;
}

.loading, .empty-state {
  text-align: center;
  padding: 40px;
  color: #666;
}

.order-card {
  border: 1px solid #ddd;
  border-radius: 4px;
  padding: 15px;
  margin-bottom: 15px;
}

.order-header {
  display: flex;
  justify-content: space-between;
  margin-bottom: 10px;
}

.order-number {
  font-weight: bold;
}

.order-status {
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 0.9em;
}

.order-status.pending { background-color: #ffd700; }
.order-status.processing { background-color: #87ceeb; }
.order-status.completed { background-color: #90ee90; }
.order-status.cancelled { background-color: #ffcccb; }

.order-info {
  margin-bottom: 10px;
}

.order-info p {
  margin: 5px 0;
  color: #666;
}

.order-actions {
  display: flex;
  gap: 10px;
}

.action-btn {
  padding: 5px 10px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.action-btn.view { background-color: #4CAF50; color: white; }
.action-btn.edit { background-color: #2196F3; color: white; }
.action-btn.delete { background-color: #f44336; color: white; }

.modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal-content {
  background: white;
  padding: 20px;
  border-radius: 8px;
  width: 90%;
  max-width: 500px;
  max-height: 90vh;
  overflow-y: auto;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
}

.form-group input {
  width: 100%;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
  margin-bottom: 10px;
}

.order-item {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr auto;
  gap: 10px;
  margin-bottom: 10px;
}

.add-btn, .remove-btn {
  padding: 5px 10px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.add-btn {
  background-color: #4CAF50;
  color: white;
}

.remove-btn {
  background-color: #f44336;
  color: white;
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
}

.modal-actions button {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.modal-actions button:first-child {
  background-color: #4CAF50;
  color: white;
}

.modal-actions button:last-child {
  background-color: #f44336;
  color: white;
}

.items-list {
  margin-top: 10px;
}

.item {
  display: flex;
  justify-content: space-between;
  padding: 5px 0;
  border-bottom: 1px solid #eee;
}
</style> 