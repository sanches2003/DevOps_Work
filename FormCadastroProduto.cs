using Repositorio.Entity;
using Repositorio;
using SalesProject.Consulta;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesProject.Cadastro
{
    public partial class FormCadastroProduto : Form
    {
        private void limparCampos()
        {
            this.txtCodigo.Clear();
            this.txtNome.Clear();
            this.txtDescricao.Clear();
            this.txtQuantidade.Clear();
            this.txtValor.Clear();
        }

        private void habilitarCampos(bool habilita)
        {
            this.txtNome.Enabled = habilita;
            this.CbTipoProduto.Enabled = habilita;
            this.txtDescricao.Enabled = habilita;
            this.txtQuantidade.Enabled = habilita;
            this.CbFornecedor.Enabled = habilita;
            this.txtValor.Enabled = habilita;
        }

        private void controlarBotoes(OperadorEnum acao)
        {
            btnCancelar.Enabled = false;
            btnDeletar.Enabled = false;
            btnNovo.Enabled = false;
            btnSalvar.Enabled = false;

            if (acao == OperadorEnum.alterar)
            {
                btnCancelar.Enabled = true;
                btnDeletar.Enabled = true;
                btnSalvar.Enabled = true;
            }
            else
            {
                if (acao == OperadorEnum.inserir)
                {
                    btnCancelar.Enabled = true;
                    btnSalvar.Enabled = true;
                }
                else
                {
                    if (acao == OperadorEnum.inicial)
                    {
                        btnNovo.Enabled = true;
                    }
                }
            }
        }

        private void eventoBotaoNovoClick(object sender, EventArgs e)
        {
            txtCodigo.Enabled = false;

            txtCodigo.Text = "0";

            this.controlarBotoes(OperadorEnum.inserir);

            this.habilitarCampos(true);

            txtNome.Focus();
        }

        private void eventoBotaoDeletarClick(object sender, EventArgs e)
        {
            int id = int.Parse(txtCodigo.Text);

            var repositorio = new ClienteRepositorio();

            var obj = repositorio.recuperarPorId(id);

            if (obj != null)
            {
                var sucesso = repositorio.remover(id);

                if (sucesso)
                {
                    MessageBox.Show("Excluído com sucesso!");
                }
                else
                {
                    MessageBox.Show("Erro ao excluir. Tente novamente.");
                }
            }
            else
            {
                MessageBox.Show("O produto não existe.");
            }

            this.controlarBotoes(OperadorEnum.inicial);

            this.habilitarCampos(false);

            this.limparCampos();
        }

        private void eventoBotaoCancelarClick(object sender, EventArgs e)
        {
            this.controlarBotoes(OperadorEnum.inicial);

            this.habilitarCampos(false);

            this.limparCampos();
        }

        private void eventoBotaoSalvarClick(object sender, EventArgs e)
        {
            if (this.validarCamposProduto())
            {

                int id = int.Parse(txtCodigo.Text);
                var repositorio = new ProdutoRepositorio();

                Produto obj = new Produto();

                obj.Id = id;
                obj.Nome = txtNome.Text;
                obj.IdTipoProduto = ((TipoProduto)CbTipoProduto.Items[CbTipoProduto.SelectedIndex]).Id;
                obj.Descricao = txtDescricao.Text;
                obj.Quantidade = int.Parse(txtQuantidade.Text);
                obj.IdFornecedor = ((Fornecedor)CbFornecedor.Items[CbFornecedor.SelectedIndex]).Id;
                obj.Valor = Decimal.Parse(txtValor.Text);

                var sucesso = repositorio.salvar(obj);

                if (sucesso)
                {
                    MessageBox.Show("Salvo com sucesso!");

                }
                else
                {
                    MessageBox.Show("Erro ao salvar. Tente novamente.");
                }

                this.controlarBotoes(OperadorEnum.inicial);

                this.habilitarCampos(false);

                this.limparCampos();
            }
        }

        private void eventoBotaoLocalizarClick(object sender, EventArgs e)
        {
            using (var frm = new FormConsultaProduto())
            {
                frm.ShowDialog();

                var id = frm.obterIdSelecionado();

                txtCodigo.Text = id.ToString();

                this.controlarBotoes(OperadorEnum.alterar);

                this.habilitarCampos(true);

                this.carregarDados(id);

                txtNome.Focus();
            }
        }
        private void carregarFornecedor()
        {
            var repositorio = new FornecedorRepositorio();

            var listafornecedores = repositorio.listarTodos();

            CbFornecedor.DataSource = listafornecedores;
            CbFornecedor.DisplayMember = "Nome";
            CbFornecedor.ValueMember = "Id";
        }

        private void carregarCodigoTipoProduto()
        {
            var repositorio = new TipoProdutoRepositorio();

            var listatiposprodutos = repositorio.listarTodos();
            
// caso queira que na sua combo box, ela deixe de exibir "NomeTipoProduto" para exibir a "Descricao", altere assim:
            CbTipoProduto.DataSource = listatiposprodutos;
            CbTipoProduto.DisplayMember = "DescricaoTipoProduto";
            CbTipoProduto.ValueMember = "Id";
        }

        public void carregarDados(int id)
        {
            var repositorio = new ProdutoRepositorio();

            var obj = repositorio.recuperarPorId(id);

            if (obj != null)
            {
                txtNome.Text = obj.Nome;
                CbTipoProduto.Text = obj.IdTipoProduto.ToString();
                txtDescricao.Text = obj.Descricao;
                txtQuantidade.Text = obj.Quantidade.ToString();
                CbFornecedor.Text = obj.IdFornecedor.ToString();
                txtValor.Text = obj.Valor.ToString();
            }
        }

        private bool validarValor()
        {

            var sucesso = true;

            try
            {
                decimal.Parse(txtValor.Text);
            }
            catch (Exception)
            {
                sucesso = false;

            }

            return sucesso;
        }


        private bool validarCamposProduto()
        {
            bool sucesso = true;

            if (string.IsNullOrEmpty(txtNome.Text))
            {
                MessageBox.Show("Informe o nome!");

                return false;
            }

            if (string.IsNullOrEmpty(CbTipoProduto.Text))
            {
                MessageBox.Show("Informe o tipo de produto!");

                return false;
            }

            if (string.IsNullOrEmpty(txtDescricao.Text))
            {
                MessageBox.Show("Informe a descrição do produto!");

                return false;
            }

            if (string.IsNullOrEmpty(txtQuantidade.Text))
            {
                MessageBox.Show("Informe a quantidade!");

                return false;
            }

            if (string.IsNullOrEmpty(CbFornecedor.Text))
            {
                MessageBox.Show("Informe o fornecedor!");

                return false;
            }

            if (string.IsNullOrEmpty(txtValor.Text))
            {
                MessageBox.Show("Informe o valor!");

                return false;
            }

            if (!this.validarValor())
            {
                sucesso = false;

                MessageBox.Show("Informe um valor válido!");

                txtValor.Text = "0";

                txtValor.Focus();
            }

            return sucesso;
        }
        public FormCadastroProduto()
        {
            InitializeComponent();

            this.txtCodigo.Enabled = false;

            this.habilitarCampos(false);

            this.carregarCodigoTipoProduto();

            this.carregarFornecedor();

            this.controlarBotoes(OperadorEnum.inicial);

            this.btnNovo.Click += this.eventoBotaoNovoClick;

            this.btnDeletar.Click += this.eventoBotaoDeletarClick;

            this.btnCancelar.Click += this.eventoBotaoCancelarClick;

            this.btnSalvar.Click += this.eventoBotaoSalvarClick;

            this.btnLocalizar.Click += this.eventoBotaoLocalizarClick;
        }

    }
}
