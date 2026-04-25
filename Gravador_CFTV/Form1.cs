using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms; // Certifique-se de ter o pacote WinForms instalado

namespace Gravador_CFTV
{
    public partial class Form1 : Form
    {
        public LibVLC _libVLC;
        public MediaPlayer _mediaPlayer;

        public Form1()
        {
            InitializeComponent();
            Core.Initialize(); // Inicializa os binários do VLC
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            // Conecta o player ao componente VideoView da sua tela
            videoView1.MediaPlayer = _mediaPlayer;
        }

        private void LoadCombos()
        {
            cbxDeviceType.DataSource = EnumHelper.ToList<AppEnums.DeviceType>();
            cbxDeviceType.DisplayMember = "Text";
            cbxDeviceType.ValueMember = "Value";
            cbxDeviceType.SelectedIndex = -1;

            cbxManufacturer.DataSource = EnumHelper.ToList<AppEnums.Manufacturer>();
            cbxManufacturer.DisplayMember = "Text";
            cbxManufacturer.ValueMember = "Value";
            cbxManufacturer.SelectedIndex = -1;

            cbxStream.DataSource = EnumHelper.ToList<AppEnums.StreamType>();
            cbxStream.DisplayMember = "Text";
            cbxStream.ValueMember = "Value";
            cbxStream.SelectedIndex = -1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCombos();
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbxDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDeviceType.SelectedValue == null)
                return;

            // Garante que o valor é do tipo enum
            if (!(cbxDeviceType.SelectedValue is AppEnums.DeviceType tipo))
                return;

            cbxChannel.Items.Clear();

            if (tipo == AppEnums.DeviceType.CameraIP)
            {
                cbxChannel.Items.Add("Canal 1");
                cbxChannel.SelectedIndex = 0;
                cbxChannel.Enabled = false; // opcional (melhor UX)
            }
            else
            {
                cbxChannel.Items.AddRange(
                    Enumerable.Range(1, 64)
                              .Select(i => $"Canal {i}")
                              .ToArray()
                );

                cbxChannel.SelectedIndex = 0;
                cbxChannel.Enabled = true;
            }
        }

        private void cbxManufacturer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbxChannel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtIp_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtConfpass_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTestPort_Click(object sender, EventArgs e)
        {
            string ip = txtIp.Text;
            int port = int.Parse(txtPort.Text);

            // Valida se a porta é um número
            if (!int.TryParse(txtPort.Text, out port))
            {
                MessageBox.Show("Por favor, insira uma porta válida.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (TcpClient client = new TcpClient())
                {
                    // Tenta conectar com um timeout curto (ex: 3 segundos)
                    var result = client.BeginConnect(ip, port, null, null);
                    // Aguarda o resultado da conexão
                    bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3));

                    if (success && client.Connected)
                    {
                        MessageBox.Show($"Sucesso! O dispositivo em {ip}:{port} está online.",
                            "Teste de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Aqui você pode mudar a cor de um label de "Notificações" para verde, por exemplo.
                    }
                    else
                    {
                        throw new Exception("Tempo de resposta esgotado.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha na conexão: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
