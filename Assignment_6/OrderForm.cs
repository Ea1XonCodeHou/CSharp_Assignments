using System;
using System.Windows.Forms;
using Assignment_6.Models;

namespace Assignment_6
{
    public partial class OrderForm : Form
    {
        public Order Order { get; private set; }
        private BindingSource detailBindingSource;

        public OrderForm(Order order = null)
        {
            InitializeComponent();
            Order = order ?? new Order();
            InitializeBindings();
        }

        private void InitializeBindings()
        {
            // 绑定订单基本信息
            txtOrderId.DataBindings.Add("Text", Order, "OrderId", true, DataSourceUpdateMode.Never);
            txtCustomerName.DataBindings.Add("Text", Order, "CustomerName");
            dtpOrderDate.DataBindings.Add("Value", Order, "OrderDate");

            // 初始化订单明细数据源
            detailBindingSource = new BindingSource();
            detailBindingSource.DataSource = Order.Details;

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
        }

        private void btnAddDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtProductName.Text))
                {
                    MessageBox.Show("请输入商品名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) || unitPrice <= 0)
                {
                    MessageBox.Show("请输入有效的单价", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("请输入有效的数量", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var detail = new OrderDetail(txtProductName.Text, unitPrice, quantity);
                Order.AddDetail(detail);
                detailBindingSource.ResetBindings(false);

                // 清空输入框
                txtProductName.Clear();
                txtUnitPrice.Clear();
                txtQuantity.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveDetail_Click(object sender, EventArgs e)
        {
            if (dgvDetails.CurrentRow == null)
            {
                MessageBox.Show("请先选择要删除的明细", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var detail = dgvDetails.CurrentRow.DataBoundItem as OrderDetail;
            if (detail != null)
            {
                Order.RemoveDetail(detail);
                detailBindingSource.ResetBindings(false);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Order.CustomerName))
            {
                MessageBox.Show("请输入客户名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Order.Details.Count == 0)
            {
                MessageBox.Show("请添加至少一个订单明细", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
} 