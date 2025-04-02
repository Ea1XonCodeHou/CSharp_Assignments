using System;
using System.Windows.Forms;
using Assignment_5.model;
using Assignment_5.common;

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
            // �󶨶���������Ϣ
            txtOrderId.DataBindings.Add("Text", Order, "OrderId", true, DataSourceUpdateMode.Never);
            txtCustomerName.DataBindings.Add("Text", Order, "CustomerName");
            dtpOrderDate.DataBindings.Add("Value", Order, "OrderDate");

            // ��ʼ��������ϸ����Դ
            detailBindingSource = new BindingSource();
            detailBindingSource.DataSource = Order.Details;

            // �󶨶�����ϸ�б�
            dgvDetails.DataSource = detailBindingSource;
            dgvDetails.AutoGenerateColumns = false;
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "��Ʒ����",
                DataPropertyName = "ProductName"
            });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitPrice",
                HeaderText = "����",
                DataPropertyName = "UnitPrice"
            });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "����",
                DataPropertyName = "Quantity"
            });
        }

        private void btnAddDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtProductName.Text))
                {
                    MessageBox.Show("��������Ʒ����", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) || unitPrice <= 0)
                {
                    MessageBox.Show("��������Ч�ĵ���", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("��������Ч������", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var detail = new OrderDetail(txtProductName.Text, unitPrice, quantity);
                Order.AddDetail(detail);
                detailBindingSource.ResetBindings(false);

                // ��������
                txtProductName.Clear();
                txtUnitPrice.Clear();
                txtQuantity.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveDetail_Click(object sender, EventArgs e)
        {
            if (dgvDetails.CurrentRow == null)
            {
                MessageBox.Show("����ѡ��Ҫɾ������ϸ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var detail = (OrderDetail)dgvDetails.CurrentRow.DataBoundItem;
            try
            {
                Order.RemoveDetail(detail.ProductName);
                detailBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("������ͻ�����", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Order.Details.Count == 0)
            {
                MessageBox.Show("���������һ��������ϸ", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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