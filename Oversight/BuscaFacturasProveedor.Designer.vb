<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BuscaFacturasProveedor
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BuscaFacturasProveedor))
        Me.gbBuscar = New System.Windows.Forms.GroupBox
        Me.btnExportar = New System.Windows.Forms.Button
        Me.btnCrear = New System.Windows.Forms.Button
        Me.dgvFacturas = New System.Windows.Forms.DataGridView
        Me.btnAbrir = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.lblBuscar = New System.Windows.Forms.Label
        Me.txtBuscar = New System.Windows.Forms.TextBox
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.msSaveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.gbBuscar.SuspendLayout()
        CType(Me.dgvFacturas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbBuscar
        '
        Me.gbBuscar.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbBuscar.Controls.Add(Me.btnExportar)
        Me.gbBuscar.Controls.Add(Me.btnCrear)
        Me.gbBuscar.Controls.Add(Me.dgvFacturas)
        Me.gbBuscar.Controls.Add(Me.btnAbrir)
        Me.gbBuscar.Controls.Add(Me.btnCancelar)
        Me.gbBuscar.Controls.Add(Me.lblBuscar)
        Me.gbBuscar.Controls.Add(Me.txtBuscar)
        Me.gbBuscar.Location = New System.Drawing.Point(7, 2)
        Me.gbBuscar.Name = "gbBuscar"
        Me.gbBuscar.Size = New System.Drawing.Size(1081, 433)
        Me.gbBuscar.TabIndex = 29
        Me.gbBuscar.TabStop = False
        '
        'btnExportar
        '
        Me.btnExportar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExportar.Enabled = False
        Me.btnExportar.Image = Global.Oversight.My.Resources.Resources.excel24x24
        Me.btnExportar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportar.Location = New System.Drawing.Point(229, 393)
        Me.btnExportar.Name = "btnExportar"
        Me.btnExportar.Size = New System.Drawing.Size(107, 34)
        Me.btnExportar.TabIndex = 32
        Me.btnExportar.Text = "Exportar lista"
        Me.btnExportar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportar.UseVisualStyleBackColor = True
        '
        'btnCrear
        '
        Me.btnCrear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCrear.Enabled = False
        Me.btnCrear.Image = Global.Oversight.My.Resources.Resources.empresa24x24
        Me.btnCrear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCrear.Location = New System.Drawing.Point(11, 393)
        Me.btnCrear.Name = "btnCrear"
        Me.btnCrear.Size = New System.Drawing.Size(212, 34)
        Me.btnCrear.TabIndex = 4
        Me.btnCrear.Text = "C&rear Nueva Factura de Proveedor"
        Me.btnCrear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCrear.UseVisualStyleBackColor = True
        '
        'dgvFacturas
        '
        Me.dgvFacturas.AllowUserToAddRows = False
        Me.dgvFacturas.AllowUserToDeleteRows = False
        Me.dgvFacturas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvFacturas.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.dgvFacturas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFacturas.Location = New System.Drawing.Point(11, 47)
        Me.dgvFacturas.MultiSelect = False
        Me.dgvFacturas.Name = "dgvFacturas"
        Me.dgvFacturas.ReadOnly = True
        Me.dgvFacturas.RowHeadersVisible = False
        Me.dgvFacturas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFacturas.Size = New System.Drawing.Size(1064, 340)
        Me.dgvFacturas.TabIndex = 2
        '
        'btnAbrir
        '
        Me.btnAbrir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAbrir.Enabled = False
        Me.btnAbrir.Image = Global.Oversight.My.Resources.Resources.open24x24
        Me.btnAbrir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAbrir.Location = New System.Drawing.Point(897, 393)
        Me.btnAbrir.Name = "btnAbrir"
        Me.btnAbrir.Size = New System.Drawing.Size(178, 34)
        Me.btnAbrir.TabIndex = 6
        Me.btnAbrir.Text = "&Usar Factura Seleccionado"
        Me.btnAbrir.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAbrir.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(802, 393)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelar.TabIndex = 5
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'lblBuscar
        '
        Me.lblBuscar.AutoSize = True
        Me.lblBuscar.Location = New System.Drawing.Point(8, 18)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(43, 13)
        Me.lblBuscar.TabIndex = 25
        Me.lblBuscar.Text = "Buscar:"
        '
        'txtBuscar
        '
        Me.txtBuscar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBuscar.Location = New System.Drawing.Point(57, 15)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(1018, 20)
        Me.txtBuscar.TabIndex = 1
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'BuscaFacturasProveedor
        '
        Me.AcceptButton = Me.btnAbrir
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(1095, 438)
        Me.Controls.Add(Me.gbBuscar)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "BuscaFacturasProveedor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Buscar Factura de Proveedor"
        Me.gbBuscar.ResumeLayout(False)
        Me.gbBuscar.PerformLayout()
        CType(Me.dgvFacturas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbBuscar As System.Windows.Forms.GroupBox
    Friend WithEvents btnAbrir As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblBuscar As System.Windows.Forms.Label
    Friend WithEvents txtBuscar As System.Windows.Forms.TextBox
    Friend WithEvents dgvFacturas As System.Windows.Forms.DataGridView
    Friend WithEvents btnCrear As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents btnExportar As System.Windows.Forms.Button
    Friend WithEvents msSaveFileDialog As System.Windows.Forms.SaveFileDialog
End Class
