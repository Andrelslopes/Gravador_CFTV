using LibVLCSharp.Shared;
using LibVLCSharp.WinForms; // Certifique-se de ter o pacote WinForms instalado
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
using static Gravador_CFTV.AppEnums;

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

        public string GetRtspUrl(DeviceInformation device)
        {
            switch (device.Manufacturer)
            {
                case Manufacturer.Hikvision:

                    string hikStream =
                        device.Stream == StreamType.Main ? "01" : "02";

                    return $"rtsp://{device.Username}:{device.Password}@{device.IP}:{device.Port}/Streaming/Channels/{device.Channel}{hikStream}";

                case Manufacturer.Intelbras:
                case Manufacturer.Dahua:

                    string subtype =
                        device.Stream == StreamType.Main ? "0" : "1";

                    return $"rtsp://{device.Username}:{device.Password}@{device.IP}:{device.Port}/cam/realmonitor?channel={device.Channel}&subtype={subtype}";

                case Manufacturer.Other:
                    return device.CustomUrl;

                default:
                    return string.Empty;
            }
        }

        private void TestarStream(string url)
        {
            try
            {
                Core.Initialize();

                _libVLC = new LibVLC(enableDebugLogs: true);

                _libVLC.Log += (sender, e) =>
                {
                    Invoke(new Action(() =>
                    {
                        txtLogs.AppendText(
                            $"[{e.Level}] {e.Module}: {e.Message}" +
                            Environment.NewLine);
                    }));
                };

                _mediaPlayer = new MediaPlayer(_libVLC);

                videoView1.MediaPlayer = _mediaPlayer;

                _mediaPlayer.Playing += (s, e) =>
                {
                    Invoke(new Action(() =>
                    {
                        lblNotification.Text = "Conectado";
                        lblNotification.BackColor = ColorTranslator.FromHtml("#32CD32"); // LimeGreen
                    }));
                };

                _mediaPlayer.EncounteredError += (s, e) =>
                {
                    Invoke(new Action(() =>
                    {
                        lblNotification.Text = "Erro ao conectar";
                        lblNotification.BackColor = ColorTranslator.FromHtml("#FF6347"); // Tomate (vermelho)
                    }));
                };

                var media = new Media(_libVLC, new Uri(url));

                _mediaPlayer.Play(media);

                lblNotification.Text = "Conectando...";
                lblNotification.BackColor = ColorTranslator.FromHtml("#FFA500"); // Laranja
            }
            catch (Exception ex)
            {
                lblNotification.Text = ex.Message;
            }
        }

        private void ResetarPlayer()
        {
            if (videoView1.MediaPlayer != null)
            {
                try
                {
                    videoView1.MediaPlayer.Stop();
                    videoView1.MediaPlayer.Dispose();
                }
                catch { }
            }

            videoView1.MediaPlayer = new MediaPlayer(_libVLC);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCombos();
            txtPass.UseSystemPasswordChar = true;
            txtConfpass.UseSystemPasswordChar = true;
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {
            txtIp.BackColor = SystemColors.Window; // Cor padrão
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            txtName .BackColor = SystemColors.Window; // Cor padrão
        }

        private void cbxDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxDeviceType.BackColor = SystemColors.Window; // Cor padrão

            if (cbxDeviceType.SelectedValue == null)
                return;

            // Garante que o valor é do tipo enum
            if (!(cbxDeviceType.SelectedValue is AppEnums.DeviceType tipo))
                return;

            cbxChannel.Items.Clear();

            if (tipo == AppEnums.DeviceType.CameraIP)
            {
                cbxChannel.Items.Add("Canal 1");
                cbxChannel.SelectedIndex = -1;
                //cbxChannel.Enabled = false; // opcional (melhor UX)
            }
            else
            {
                cbxChannel.Items.AddRange(
                    Enumerable.Range(1, 64)
                              .Select(i => $"Canal {i}")
                              .ToArray()
                );

                cbxChannel.SelectedIndex = -1;
                cbxChannel.Enabled = true;
            }
        }

        private void cbxManufacturer_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxManufacturer.BackColor = SystemColors.Window; // Cor padrão
        }

        private void cbxChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxChannel.BackColor = SystemColors.Window; // Cor padrão
        }

        private void txtIp_TextChanged(object sender, EventArgs e)
        {
            txtIp.BackColor = SystemColors.Window; // Cor padrão
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            txtPort.BackColor = SystemColors.Window; // Cor padrão
        }
        private void cbxStream_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxStream.BackColor = SystemColors.Window; // Cor padrão
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            txtUser.BackColor = SystemColors.Window; // Cor padrão
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            txtPass.BackColor = SystemColors.Window; // Cor padrão
        }

        private void txtConfpass_TextChanged(object sender, EventArgs e)
        {
            txtConfpass.BackColor = SystemColors.Window; // Cor padrão
        }

        private void btnTestPort_Click(object sender, EventArgs e)
        {
            string ip = txtIp.Text;

            // Valida se o IP não está vazio ou nulo
            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show("Por favor, insira um endereço IP válido.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Valida se a porta é um número
            if (!int.TryParse(txtPort.Text, out int port))
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

            string errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errorMessage += "Nome.\n";
                txtName.BackColor = ColorTranslator.FromHtml("#FEC6C6"); // Vermelho claro
            }

            if (cbxDeviceType.SelectedValue == null)
            {
                errorMessage += "Tipo de Dispositivo.\n";
                cbxDeviceType.BackColor = ColorTranslator.FromHtml("#FEC6C6"); // Vermelho claro
            }

            if (cbxManufacturer.SelectedValue == null)
            {
                errorMessage += "Fabricante.\n";
                cbxManufacturer.BackColor = ColorTranslator.FromHtml("#FEC6C6");
            }

            if (cbxChannel.SelectedItem == null)
            {
                errorMessage += "Canal.\n";
                cbxChannel.BackColor = ColorTranslator.FromHtml("#FEC6C6");
            }

            if (string.IsNullOrWhiteSpace(txtIp.Text))
            {
                errorMessage += "IP.\n";
                txtIp.BackColor = ColorTranslator.FromHtml("#FEC6C6"); // Vermelho claro
            }

            if (string.IsNullOrWhiteSpace(txtPort.Text))
            {
                errorMessage += "Porta.\n";
                txtPort.BackColor = ColorTranslator.FromHtml("#FEC6C6");
            }

            if (cbxStream.SelectedValue == null)
            {
                errorMessage += "Tipo de Stream.\n";
                cbxStream.BackColor = ColorTranslator.FromHtml("#FEC6C6");
            }

            if (string.IsNullOrWhiteSpace(txtUser.Text))
            {
                errorMessage += "Usuário.\n";
                txtUser.BackColor = ColorTranslator.FromHtml("#FEC6C6");
            }

            if (string.IsNullOrWhiteSpace(txtPass.Text))
            {
                errorMessage += "Senha.\n";
                txtPass.BackColor = ColorTranslator.FromHtml("#FEC6C6");
            }

            if (string.IsNullOrWhiteSpace(txtConfpass.Text))
            {
                errorMessage += "Confirmação de Senha.\n";
                txtConfpass.BackColor = ColorTranslator.FromHtml("#FEC6C6");
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show($"Os seguintes campos são obrigatórios:\n\n{errorMessage}",
                    "Campos Obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                ResetarPlayer();

                Manufacturer manufacturer;

                if (!Enum.TryParse(cbxManufacturer.SelectedValue.ToString(), out manufacturer))
                {
                    MessageBox.Show("Fabricante inválido.");
                    return;
                }

                var device = new DeviceInformation
                {
                    IP = txtIp.Text,
                    Port = int.Parse(txtPort.Text),
                    Username = txtUser.Text,
                    Password = txtPass.Text,
                    Channel = Convert.ToInt32(cbxChannel.SelectedItem.ToString().Replace("Canal ", "")),
                    Manufacturer = manufacturer,
                    Stream = (StreamType)cbxStream.SelectedValue,
                    CustomUrl = txtRtspUrl.Text
                };

                string rtspUrl = GetRtspUrl(device);

                txtRtspUrl.Text = rtspUrl;

                TestarStream(rtspUrl);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void txtConfpass_Leave(object sender, EventArgs e)
        {
            if (txtPass.Text != txtConfpass.Text)
            {
                txtConfpass.BackColor = ColorTranslator.FromHtml("#FEC6C6"); // Vermelho claro
            }
            else
            {
                txtConfpass.BackColor = SystemColors.Window; // Cor padrão
            }
        }
    }
}
