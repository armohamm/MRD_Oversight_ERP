<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarPago
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarPago))
        Me.gbPago = New System.Windows.Forms.GroupBox
        Me.btnPersonas = New System.Windows.Forms.Button
        Me.btnBancoDestino = New System.Windows.Forms.Button
        Me.txtNombrePersona = New System.Windows.Forms.TextBox
        Me.lblPersona = New System.Windows.Forms.Label
        Me.btnCuentaOrigen = New System.Windows.Forms.Button
        Me.btnTipoPago = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtReferenciaDestino = New System.Windows.Forms.TextBox
        Me.txtCuentaDestino = New System.Windows.Forms.TextBox
        Me.lblCantidad = New System.Windows.Forms.Label
        Me.txtImporte = New System.Windows.Forms.TextBox
        Me.lblBancoDestino = New System.Windows.Forms.Label
        Me.cmbBancoDestino = New System.Windows.Forms.ComboBox
        Me.lblCuentaDestino = New System.Windows.Forms.Label
        Me.lblCuentaSalida = New System.Windows.Forms.Label
        Me.cmbCuentaOrigen = New System.Windows.Forms.ComboBox
        Me.cmbTipoPago = New System.Windows.Forms.ComboBox
        Me.lblTipoSalida = New System.Windows.Forms.Label
        Me.lblFecha = New System.Windows.Forms.Label
        Me.dtFechaPago = New System.Windows.Forms.DateTimePicker
        Me.lblDescripcion = New System.Windows.Forms.Label
        Me.txtDescripcionPago = New System.Windows.Forms.TextBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.btnRevisiones = New System.Windows.Forms.Button
        Me.gbPago.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbPago
        '
        Me.gbPago.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPago.Controls.Add(Me.btnRevisiones)
        Me.gbPago.Controls.Add(Me.btnPersonas)
        Me.gbPago.Controls.Add(Me.btnBancoDestino)
        Me.gbPago.Controls.Add(Me.txtNombrePersona)
        Me.gbPago.Controls.Add(Me.lblPersona)
        Me.gbPago.Controls.Add(Me.btnCuentaOrigen)
        Me.gbPago.Controls.Add(Me.btnTipoPago)
        Me.gbPago.Controls.Add(Me.Label1)
        Me.gbPago.Controls.Add(Me.txtReferenciaDestino)
        Me.gbPago.Controls.Add(Me.txtCuentaDestino)
        Me.gbPago.Controls.Add(Me.lblCantidad)
        Me.gbPago.Controls.Add(Me.txtImporte)
        Me.gbPago.Controls.Add(Me.lblBancoDestino)
        Me.gbPago.Controls.Add(Me.cmbBancoDestino)
        Me.gbPago.Controls.Add(Me.lblCuentaDestino)
        Me.gbPago.Controls.Add(Me.lblCuentaSalida)
        Me.gbPago.Controls.Add(Me.cmbCuentaOrigen)
        Me.gbPago.Controls.Add(Me.cmbTipoPago)
        Me.gbPago.Controls.Add(Me.lblTipoSalida)
        Me.gbPago.Controls.Add(Me.lblFecha)
        Me.gbPago.Controls.Add(Me.dtFechaPago)
        Me.gbPago.Controls.Add(Me.lblDescripcion)
        Me.gbPago.Controls.Add(Me.txtDescripcionPago)
        Me.gbPago.Controls.Add(Me.btnGuardar)
        Me.gbPago.Controls.Add(Me.btnCancelar)
        Me.gbPago.Location = New System.Drawing.Point(3, -2)
        Me.gbPago.Name = "gbPago"
        Me.gbPago.Size = New System.Drawing.Size(494, 440)
        Me.gbPago.TabIndex = 57
        Me.gbPago.TabStop = False
        '
        'btnPersonas
        '
        Me.btnPersonas.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPersonas.Enabled = False
        Me.btnPersonas.Image = Global.Oversight.My.Resources.Resources.clients12x12
        Me.btnPersonas.Location = New System.Drawing.Point(447, 319)
        Me.btnPersonas.Name = "btnPersonas"
        Me.btnPersonas.Size = New System.Drawing.Size(27, 23)
        Me.btnPersonas.TabIndex = 13
        Me.btnPersonas.UseVisualStyleBackColor = True
        '
        'btnBancoDestino
        '
        Me.btnBancoDestino.Enabled = False
        Me.btnBancoDestino.Image = Global.Oversight.My.Resources.Resources.bank12x12
        Me.btnBancoDestino.Location = New System.Drawing.Point(417, 106)
        Me.btnBancoDestino.Name = "btnBancoDestino"
        Me.btnBancoDestino.Size = New System.Drawing.Size(27, 23)
        Me.btnBancoDestino.TabIndex = 8
        Me.btnBancoDestino.UseVisualStyleBackColor = True
        '
        'txtNombrePersona
        '
        Me.txtNombrePersona.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombrePersona.Enabled = False
        Me.txtNombrePersona.Location = New System.Drawing.Point(180, 321)
        Me.txtNombrePersona.Name = "txtNombrePersona"
        Me.txtNombrePersona.Size = New System.Drawing.Size(265, 20)
        Me.txtNombrePersona.TabIndex = 0
        Me.txtNombrePersona.TabStop = False
        '
        'lblPersona
        '
        Me.lblPersona.AutoSize = True
        Me.lblPersona.Location = New System.Drawing.Point(7, 324)
        Me.lblPersona.Name = "lblPersona"
        Me.lblPersona.Size = New System.Drawing.Size(82, 13)
        Me.lblPersona.TabIndex = 63
        Me.lblPersona.Text = "Depositado por:"
        '
        'btnCuentaOrigen
        '
        Me.btnCuentaOrigen.Enabled = False
        Me.btnCuentaOrigen.Image = Global.Oversight.My.Resources.Resources.creditcard12x12
        Me.btnCuentaOrigen.Location = New System.Drawing.Point(417, 76)
        Me.btnCuentaOrigen.Name = "btnCuentaOrigen"
        Me.btnCuentaOrigen.Size = New System.Drawing.Size(27, 23)
        Me.btnCuentaOrigen.TabIndex = 5
        Me.btnCuentaOrigen.UseVisualStyleBackColor = True
        '
        'btnTipoPago
        '
        Me.btnTipoPago.Enabled = False
        Me.btnTipoPago.Image = Global.Oversight.My.Resources.Resources.payrollSolutions12x12
        Me.btnTipoPago.Location = New System.Drawing.Point(418, 46)
        Me.btnTipoPago.Name = "btnTipoPago"
        Me.btnTipoPago.Size = New System.Drawing.Size(27, 23)
        Me.btnTipoPago.TabIndex = 3
        Me.btnTipoPago.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 169)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 87
        Me.Label1.Text = "Referencia:"
        '
        'txtReferenciaDestino
        '
        Me.txtReferenciaDestino.Location = New System.Drawing.Point(180, 166)
        Me.txtReferenciaDestino.MaxLength = 10
        Me.txtReferenciaDestino.Name = "txtReferenciaDestino"
        Me.txtReferenciaDestino.Size = New System.Drawing.Size(231, 20)
        Me.txtReferenciaDestino.TabIndex = 10
        '
        'txtCuentaDestino
        '
        Me.txtCuentaDestino.Location = New System.Drawing.Point(180, 138)
        Me.txtCuentaDestino.Name = "txtCuentaDestino"
        Me.txtCuentaDestino.Size = New System.Drawing.Size(231, 20)
        Me.txtCuentaDestino.TabIndex = 9
        '
        'lblCantidad
        '
        Me.lblCantidad.AutoSize = True
        Me.lblCantidad.Location = New System.Drawing.Point(8, 295)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(45, 13)
        Me.lblCantidad.TabIndex = 84
        Me.lblCantidad.Text = "Importe:"
        '
        'txtImporte
        '
        Me.txtImporte.Location = New System.Drawing.Point(180, 292)
        Me.txtImporte.Name = "txtImporte"
        Me.txtImporte.Size = New System.Drawing.Size(121, 20)
        Me.txtImporte.TabIndex = 12
        '
        'lblBancoDestino
        '
        Me.lblBancoDestino.AutoSize = True
        Me.lblBancoDestino.Location = New System.Drawing.Point(8, 111)
        Me.lblBancoDestino.Name = "lblBancoDestino"
        Me.lblBancoDestino.Size = New System.Drawing.Size(80, 13)
        Me.lblBancoDestino.TabIndex = 82
        Me.lblBancoDestino.Text = "Banco Destino:"
        '
        'cmbBancoDestino
        '
        Me.cmbBancoDestino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBancoDestino.FormattingEnabled = True
        Me.cmbBancoDestino.Location = New System.Drawing.Point(180, 108)
        Me.cmbBancoDestino.Name = "cmbBancoDestino"
        Me.cmbBancoDestino.Size = New System.Drawing.Size(231, 21)
        Me.cmbBancoDestino.TabIndex = 7
        '
        'lblCuentaDestino
        '
        Me.lblCuentaDestino.AutoSize = True
        Me.lblCuentaDestino.Location = New System.Drawing.Point(8, 141)
        Me.lblCuentaDestino.Name = "lblCuentaDestino"
        Me.lblCuentaDestino.Size = New System.Drawing.Size(83, 13)
        Me.lblCuentaDestino.TabIndex = 78
        Me.lblCuentaDestino.Text = "Cuenta Destino:"
        '
        'lblCuentaSalida
        '
        Me.lblCuentaSalida.AutoSize = True
        Me.lblCuentaSalida.Location = New System.Drawing.Point(8, 81)
        Me.lblCuentaSalida.Name = "lblCuentaSalida"
        Me.lblCuentaSalida.Size = New System.Drawing.Size(78, 13)
        Me.lblCuentaSalida.TabIndex = 77
        Me.lblCuentaSalida.Text = "Cuenta Origen:"
        '
        'cmbCuentaOrigen
        '
        Me.cmbCuentaOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCuentaOrigen.FormattingEnabled = True
        Me.cmbCuentaOrigen.Location = New System.Drawing.Point(181, 78)
        Me.cmbCuentaOrigen.Name = "cmbCuentaOrigen"
        Me.cmbCuentaOrigen.Size = New System.Drawing.Size(230, 21)
        Me.cmbCuentaOrigen.TabIndex = 4
        '
        'cmbTipoPago
        '
        Me.cmbTipoPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoPago.FormattingEnabled = True
        Me.cmbTipoPago.Location = New System.Drawing.Point(181, 48)
        Me.cmbTipoPago.Name = "cmbTipoPago"
        Me.cmbTipoPago.Size = New System.Drawing.Size(231, 21)
        Me.cmbTipoPago.TabIndex = 2
        '
        'lblTipoSalida
        '
        Me.lblTipoSalida.AutoSize = True
        Me.lblTipoSalida.Location = New System.Drawing.Point(9, 51)
        Me.lblTipoSalida.Name = "lblTipoSalida"
        Me.lblTipoSalida.Size = New System.Drawing.Size(74, 13)
        Me.lblTipoSalida.TabIndex = 74
        Me.lblTipoSalida.Text = "Tipo de Pago:"
        '
        'lblFecha
        '
        Me.lblFecha.AutoSize = True
        Me.lblFecha.Location = New System.Drawing.Point(9, 19)
        Me.lblFecha.Name = "lblFecha"
        Me.lblFecha.Size = New System.Drawing.Size(166, 13)
        Me.lblFecha.TabIndex = 73
        Me.lblFecha.Text = "Fecha en la que se hace el pago:"
        '
        'dtFechaPago
        '
        Me.dtFechaPago.Location = New System.Drawing.Point(181, 19)
        Me.dtFechaPago.Name = "dtFechaPago"
        Me.dtFechaPago.Size = New System.Drawing.Size(231, 20)
        Me.dtFechaPago.TabIndex = 1
        '
        'lblDescripcion
        '
        Me.lblDescripcion.AutoSize = True
        Me.lblDescripcion.Location = New System.Drawing.Point(8, 198)
        Me.lblDescripcion.Name = "lblDescripcion"
        Me.lblDescripcion.Size = New System.Drawing.Size(111, 13)
        Me.lblDescripcion.TabIndex = 63
        Me.lblDescripcion.Text = "Descripción del Pago:"
        '
        'txtDescripcionPago
        '
        Me.txtDescripcionPago.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescripcionPago.Location = New System.Drawing.Point(180, 195)
        Me.txtDescripcionPago.MaxLength = 500
        Me.txtDescripcionPago.Multiline = True
        Me.txtDescripcionPago.Name = "txtDescripcionPago"
        Me.txtDescripcionPago.Size = New System.Drawing.Size(294, 88)
        Me.txtDescripcionPago.TabIndex = 11
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.payment24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(372, 386)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(114, 34)
        Me.btnGuardar.TabIndex = 15
        Me.btnGuardar.Text = "&Guardar Pago"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(265, 386)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(101, 34)
        Me.btnCancelar.TabIndex = 14
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'btnRevisiones
        '
        Me.btnRevisiones.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRevisiones.Enabled = False
        Me.btnRevisiones.Image = Global.Oversight.My.Resources.Resources.yes24x24
        Me.btnRevisiones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevisiones.Location = New System.Drawing.Point(12, 386)
        Me.btnRevisiones.Name = "btnRevisiones"
        Me.btnRevisiones.Size = New System.Drawing.Size(111, 34)
        Me.btnRevisiones.TabIndex = 88
        Me.btnRevisiones.Text = "Revisiones"
        Me.btnRevisiones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisiones.UseVisualStyleBackColor = True
        '
        'AgregarPago
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(501, 442)
        Me.Controls.Add(Me.gbPago)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarPago"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Pago"
        Me.gbPago.ResumeLayout(False)
        Me.gbPago.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbPago As System.Windows.Forms.GroupBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblDescripcion As System.Windows.Forms.Label
    Friend WithEvents txtDescripcionPago As System.Windows.Forms.TextBox
    Friend WithEvents txtCuentaDestino As System.Windows.Forms.TextBox
    Friend WithEvents lblCantidad As System.Windows.Forms.Label
    Friend WithEvents txtCantidad As System.Windows.Forms.TextBox
    Friend WithEvents lblBancoDestino As System.Windows.Forms.Label
    Friend WithEvents cmbBancoDestino As System.Windows.Forms.ComboBox
    Friend WithEvents lblCuentaDestino As System.Windows.Forms.Label
    Friend WithEvents lblCuentaSalida As System.Windows.Forms.Label
    Friend WithEvents cmbCuentaOrigen As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTipoPago As System.Windows.Forms.ComboBox
    Friend WithEvents lblTipoSalida As System.Windows.Forms.Label
    Friend WithEvents lblFecha As System.Windows.Forms.Label
    Friend WithEvents dtFechaPago As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtReferenciaDestino As System.Windows.Forms.TextBox
    Friend WithEvents btnTipoPago As System.Windows.Forms.Button
    Friend WithEvents btnBancoDestino As System.Windows.Forms.Button
    Friend WithEvents btnCuentaOrigen As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents txtImporte As System.Windows.Forms.TextBox
    Friend WithEvents btnPersonas As System.Windows.Forms.Button
    Friend WithEvents txtNombrePersona As System.Windows.Forms.TextBox
    Friend WithEvents lblPersona As System.Windows.Forms.Label
    Friend WithEvents btnRevisiones As System.Windows.Forms.Button
End Class
