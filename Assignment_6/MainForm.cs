using System;
using System.Windows.Forms;
using Assignment_6.Models;
using Assignment_6.Services;

namespace Assignment_6
{
    public partial class MainForm : Form
    {
        private OrderService orderService;
        private BindingSource orderBindingSource;
        private BindingSource detailBindingSource;

        public MainForm()
        {
            InitializeComponent();
            orderService = new OrderService();
            InitializeBindings();
            LoadOrders();
        }

        private void InitializeBindings()
        {
            // 初始化订单数据源
            orderBindingSource = new BindingSource();
            orderBindingSource.DataSource = typeof(Order);

            // 初始化订单明细数据源
            detailBindingSource = new BindingSource();
            detailBindingSource.DataMember = "Details";
            detailBindingSource.DataSource = orderBindingSource;

            // 绑定订单列表
            dgvOrders.DataSource = orderBindingSource;
            dgvOrders.AutoGenerateColumns = false;
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderId",
                HeaderText = "订单号",
                DataPropertyName = "OrderId"
            });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CustomerName",
                HeaderText = "客户名称",
                DataPropertyName = "CustomerName"
            });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderDate",
                HeaderText = "订单日期",
                DataPropertyName = "OrderDate"
            });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalAmount",
                HeaderText = "总金额",
                DataPropertyName = "TotalAmount"
            });

            // 绑定订单明细列表
            dgvDetails.DataSource = detailBindingSource;
            dgvDetails.AutoGenerateColumns = false;
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "商品名称",
                DataPropertyName = "ProductName"
            });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitPrice",
                HeaderText = "单价",
                DataPropertyName = "UnitPrice"
            });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "数量",
                DataPropertyName = "Quantity"
            });

            // 绑定查询控件
            txtCustomerName.DataBindings.Add("Text", orderBindingSource, "CustomerName");
            txtOrderId.DataBindings.Add("Text", orderBindingSource, "OrderId");
        }

        private void LoadOrders()
        {
            orderBindingSource.DataSource = orderService.GetAllOrders();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var orderForm = new OrderForm())
            {
                if (orderForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        orderService.AddOrder(orderForm.Order);
                        LoadOrders();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (orderBindingSource.Current == null)
            {
                MessageBox.Show("请先选择一个订单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var currentOrder = (Order)orderBindingSource.Current;
            using (var orderForm = new OrderForm(currentOrder))
            {
                if (orderForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        orderService.UpdateOrder(orderForm.Order);
                        LoadOrders();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (orderBindingSource.Current == null)
            {
                MessageBox.Show("请先选择一个订单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var currentOrder = (Order)orderBindingSource.Current;
            if (MessageBox.Show($"确定要删除订单 {currentOrder.OrderId} 吗？", "确认删除",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    orderService.RemoveOrder(currentOrder.OrderId);
                    LoadOrders();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    LoadOrders();
                    return;
                }

                if (rbCustomer.Checked)
                {
                    orderBindingSource.DataSource = orderService.QueryByCustomerName(txtSearch.Text);
                }
                else if (rbProduct.Checked)
                {
                    orderBindingSource.DataSource = orderService.QueryByProductName(txtSearch.Text);
                }
                else if (rbAmount.Checked)
                {
                    if (decimal.TryParse(txtSearch.Text, out decimal amount))
                    {
                        orderBindingSource.DataSource = orderService.QueryByAmountRange(amount, amount);
                    }
                    else
                    {
                        MessageBox.Show("请输入有效的金额", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            if (rbSortId.Checked)
            {
                orderService.SortOrders(o => o.OrderId);
            }
            else if (rbSortCustomer.Checked)
            {
                orderService.SortOrders(o => o.CustomerName);
            }
            else if (rbSortAmount.Checked)
            {
                orderService.SortOrders(o => o.TotalAmount);
            }

            LoadOrders();
        }
    }
} 