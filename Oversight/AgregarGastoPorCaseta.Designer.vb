<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarGastoPorCaseta
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarGastoPorCaseta))
        Me.btnPersonas = New System.Windows.Forms.Button
        Me.txtNombrePersona = New System.Windows.Forms.TextBox
        Me.lblPersona = New System.Windows.Forms.Label
        Me.txtOrigen = New System.Windows.Forms.TextBox
        Me.lblImporte = New System.Windows.Forms.Label
        Me.txtImporte = New System.Windows.Forms.TextBox
        Me.lblCuentaOrigen = New System.Windows.Forms.Label
        Me.lblFecha = New System.Windows.Forms.Label
        Me.dtFechaTicket = New System.Windows.Forms.DateTimePicker
        Me.lblReferenciaOrigen = New System.Windows.Forms.Label
        Me.txtDestino = New System.Windows.Forms.TextBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.tcCasetas = New System.Windows.Forms.TabControl
        Me.tpCaseta = New System.Windows.Forms.TabPage
        Me.btnRevisiones = New System.Windows.Forms.Button
        Me.tpRelaciones = New System.Windows.Forms.TabPage
        Me.btnRevisionesRelaciones = New System.Windows.Forms.Button
        Me.btnCancelarRelacion = New System.Windows.Forms.Button
        Me.btnGuardarRelacion = New System.Windows.Forms.Button
        Me.btnEliminarRelacion = New System.Windows.Forms.Button
        Me.btnBuscarRelacion = New System.Windows.Forms.Button
        Me.lblItemsYaRelacionados = New System.Windows.Forms.Label
        Me.dgvConRelacion = New System.Windows.Forms.DataGridView
        Me.cmbTipoRelacion = New System.Windows.Forms.ComboBox
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.tcCasetas.SuspendLayout()
        Me.tpCaseta.SuspendLayout()
        Me.tpRelaciones.SuspendLayout()
        CType(Me.dgvConRelacion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnPersonas
        '
        Me.btnPersonas.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPersonas.Enabled = False
        Me.btnPersonas.Image = Global.Oversight.My.Resources.Resources.clients12x12
        Me.btnPersonas.Location = New System.Drawing.Point(478, 93)
        Me.btnPersonas.Name = "btnPersonas"
        Me.btnPersonas.Size = New System.Drawing.Size(27, 23)
        Me.btnPersonas.TabIndex = 13
        Me.btnPersonas.UseVisualStyleBackColor = True
        '
        'txtNombrePersona
        '
        Me.txtNombrePersona.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombrePersona.Enabled = False
        Me.txtNombrePersona.Location = New System.Drawing.Point(122, 95)
        Me.txtNombrePersona.Name = "txtNombrePersona"
        Me.txtNombrePersona.Size = New System.Drawing.Size(350, 20)
        Me.txtNombrePersona.TabIndex = 0
        Me.txtNombrePersona.TabStop = False
        '
        'lblPersona
        '
        Me.lblPersona.AutoSize = True
        Me.lblPersona.Location = New System.Drawing.Point(18, 98)
        Me.lblPersona.Name = "lblPersona"
        Me.lblPersona.Size = New System.Drawing.Size(59, 13)
        Me.lblPersona.TabIndex = 90
        Me.lblPersona.Text = "Conductor:"
        '
        'txtOrigen
        '
        Me.txtOrigen.Location = New System.Drawing.Point(122, 43)
        Me.txtOrigen.Name = "txtOrigen"
        Me.txtOrigen.Size = New System.Drawing.Size(350, 20)
        Me.txtOrigen.TabIndex = 6
        '
        'lblImporte
        '
        Me.lblImporte.AutoSize = True
        Me.lblImporte.Location = New System.Drawing.Point(18, 124)
        Me.lblImporte.Name = "lblImporte"
        Me.lblImporte.Size = New System.Drawing.Size(45, 13)
        Me.lblImporte.TabIndex = 84
        Me.lblImporte.Text = "Importe:"
        '
        'txtImporte
        '
        Me.txtImporte.Location = New System.Drawing.Point(122, 121)
        Me.txtImporte.Name = "txtImporte"
        Me.txtImporte.Size = New System.Drawing.Size(150, 20)
        Me.txtImporte.TabIndex = 12
        '
        'lblCuentaOrigen
        '
        Me.lblCuentaOrigen.AutoSize = True
        Me.lblCuentaOrigen.Location = New System.Drawing.Point(18, 46)
        Me.lblCuentaOrigen.Name = "lblCuentaOrigen"
        Me.lblCuentaOrigen.Size = New System.Drawing.Size(41, 13)
        Me.lblCuentaOrigen.TabIndex = 77
        Me.lblCuentaOrigen.Text = "Origen:"
        '
        'lblFecha
        '
        Me.lblFecha.AutoSize = True
        Me.lblFecha.Location = New System.Drawing.Point(18, 17)
        Me.lblFecha.Name = "lblFecha"
        Me.lblFecha.Size = New System.Drawing.Size(86, 13)
        Me.lblFecha.TabIndex = 73
        Me.lblFecha.Text = "Fecha del ticket:"
        '
        'dtFechaTicket
        '
        Me.dtFechaTicket.Location = New System.Drawing.Point(122, 17)
        Me.dtFechaTicket.Name = "dtFechaTicket"
        Me.dtFechaTicket.Size = New System.Drawing.Size(231, 20)
        Me.dtFechaTicket.TabIndex = 1
        '
        'lblReferenciaOrigen
        '
        Me.lblReferenciaOrigen.AutoSize = True
        Me.lblReferenciaOrigen.Location = New System.Drawing.Point(18, 72)
        Me.lblReferenciaOrigen.Name = "lblReferenciaOrigen"
        Me.lblReferenciaOrigen.Size = New System.Drawing.Size(46, 13)
        Me.lblReferenciaOrigen.TabIndex = 61
        Me.lblReferenciaOrigen.Text = "Destino:"
        '
        'txtDestino
        '
        Me.txtDestino.Location = New System.Drawing.Point(122, 69)
        Me.txtDestino.MaxLength = 10
        Me.txtDestino.Name = "txtDestino"
        Me.txtDestino.Size = New System.Drawing.Size(350, 20)
        Me.txtDestino.TabIndex = 7
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.save24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(351, 204)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(121, 34)
        Me.btnGuardar.TabIndex = 15
        Me.btnGuardar.Text = "&Guardar Caseta"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(244, 204)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(101, 34)
        Me.btnCancelar.TabIndex = 14
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'tcCasetas
        '
        Me.tcCasetas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcCasetas.Controls.Add(Me.tpCaseta)
        Me.tcCasetas.Controls.Add(Me.tpRelaciones)
        Me.tcCasetas.Location = New System.Drawing.Point(5, 5)
        Me.tcCasetas.Name = "tcCasetas"
        Me.tcCasetas.SelectedIndex = 0
        Me.tcCasetas.Size = New System.Drawing.Size(519, 277)
        Me.tcCasetas.TabIndex = 58
        '
        'tpCaseta
        '
        Me.tpCaseta.Controls.Add(Me.btnRevisiones)
        Me.tpCaseta.Controls.Add(Me.btnPersonas)
        Me.tpCaseta.Controls.Add(Me.lblFecha)
        Me.tpCaseta.Controls.Add(Me.txtNombrePersona)
        Me.tpCaseta.Controls.Add(Me.btnCancelar)
        Me.tpCaseta.Controls.Add(Me.lblPersona)
        Me.tpCaseta.Controls.Add(Me.btnGuardar)
        Me.tpCaseta.Controls.Add(Me.txtDestino)
        Me.tpCaseta.Controls.Add(Me.lblReferenciaOrigen)
        Me.tpCaseta.Controls.Add(Me.dtFechaTicket)
        Me.tpCaseta.Controls.Add(Me.txtOrigen)
        Me.tpCaseta.Controls.Add(Me.lblImporte)
        Me.tpCaseta.Controls.Add(Me.txtImporte)
        Me.tpCaseta.Controls.Add(Me.lblCuentaOrigen)
        Me.tpCaseta.Location = New System.Drawing.Point(4, 22)
        Me.tpCaseta.Name = "tpCaseta"
        Me.tpCaseta.Padding = New System.Windows.Forms.Padding(3)
        Me.tpCaseta.Size = New System.Drawing.Size(511, 251)
        Me.tpCaseta.TabIndex = 0
        Me.tpCaseta.Text = "Datos"
        Me.tpCaseta.UseVisualStyleBackColor = True
        '
        'btnRevisiones
        '
        Me.btnRevisiones.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRevisiones.Enabled = False
        Me.btnRevisiones.Image = Global.Oversight.My.Resources.Resources.yes24x24
        Me.btnRevisiones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevisiones.Location = New System.Drawing.Point(9, 204)
        Me.btnRevisiones.Name = "btnRevisiones"
        Me.btnRevisiones.Size = New System.Drawing.Size(111, 34)
        Me.btnRevisiones.TabIndex = 91
        Me.btnRevisiones.Text = "Revisiones"
        Me.btnRevisiones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisiones.UseVisualStyleBackColor = True
        '
        'tpRelaciones
        '
        Me.tpRelaciones.Controls.Add(Me.btnRevisionesRelaciones)
        Me.tpRelaciones.Controls.Add(Me.btnCancelarRelacion)
        Me.tpRelaciones.Controls.Add(Me.btnGuardarRelacion)
        Me.tpRelaciones.Controls.Add(Me.btnEliminarRelacion)
        Me.tpRelaciones.Controls.Add(Me.btnBuscarRelacion)
        Me.tpRelaciones.Controls.Add(Me.lblItemsYaRelacionados)
        Me.tpRelaciones.Controls.Add(Me.dgvConRelacion)
        Me.tpRelaciones.Controls.Add(Me.cmbTipoRelacion)
        Me.tpRelaciones.Location = New System.Drawing.Point(4, 22)
        Me.tpRelaciones.Name = "tpRelaciones"
        Me.tpRelaciones.Padding = New System.Windows.Forms.Padding(3)
        Me.tpRelaciones.Size = New System.Drawing.Size(511, 251)
        Me.tpRelaciones.TabIndex = 1
        Me.tpRelaciones.Text = "Relacionar A"
        Me.tpRelaciones.UseVisualStyleBackColor = True
        '
        'btnRevisionesRelaciones
        '
        Me.btnRevisionesRelaciones.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRevisionesRelaciones.Enabled = False
        Me.btnRevisionesRelaciones.Image = Global.Oversight.My.Resources.Resources.yes24x24
        Me.btnRevisionesRelaciones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevisionesRelaciones.Location = New System.Drawing.Point(5, 204)
        Me.btnRevisionesRelaciones.Name = "btnRevisionesRelaciones"
        Me.btnRevisionesRelaciones.Size = New System.Drawing.Size(111, 34)
        Me.btnRevisionesRelaciones.TabIndex = 92
        Me.btnRevisionesRelaciones.Text = "Revisiones"
        Me.btnRevisionesRelaciones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisionesRelaciones.UseVisualStyleBackColor = True
        '
        'btnCancelarRelacion
        '
        Me.btnCancelarRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelarRelacion.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelarRelacion.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelarRelacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelarRelacion.Location = New System.Drawing.Point(244, 204)
        Me.btnCancelarRelacion.Name = "btnCancelarRelacion"
        Me.btnCancelarRelacion.Size = New System.Drawing.Size(101, 34)
        Me.btnCancelarRelacion.TabIndex = 31
        Me.btnCancelarRelacion.Text = "&Cancelar"
        Me.btnCancelarRelacion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelarRelacion.UseVisualStyleBackColor = True
        '
        'btnGuardarRelacion
        '
        Me.btnGuardarRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardarRelacion.Enabled = False
        Me.btnGuardarRelacion.Image = Global.Oversight.My.Resources.Resources.save24x24
        Me.btnGuardarRelacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardarRelacion.Location = New System.Drawing.Point(351, 204)
        Me.btnGuardarRelacion.Name = "btnGuardarRelacion"
        Me.btnGuardarRelacion.Size = New System.Drawing.Size(121, 34)
        Me.btnGuardarRelacion.TabIndex = 32
        Me.btnGuardarRelacion.Text = "&Guardar Caseta"
        Me.btnGuardarRelacion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardarRelacion.UseVisualStyleBackColor = True
        '
        'btnEliminarRelacion
        '
        Me.btnEliminarRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarRelacion.Enabled = False
        Me.btnEliminarRelacion.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarRelacion.Location = New System.Drawing.Point(474, 28)
        Me.btnEliminarRelacion.Name = "btnEliminarRelacion"
        Me.btnEliminarRelacion.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarRelacion.TabIndex = 28
        Me.btnEliminarRelacion.UseVisualStyleBackColor = True
        '
        'btnBuscarRelacion
        '
        Me.btnBuscarRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnBuscarRelacion.Enabled = False
        Me.btnBuscarRelacion.Image = Global.Oversight.My.Resources.Resources.search12x12
        Me.btnBuscarRelacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBuscarRelacion.Location = New System.Drawing.Point(338, 149)
        Me.btnBuscarRelacion.Name = "btnBuscarRelacion"
        Me.btnBuscarRelacion.Size = New System.Drawing.Size(130, 23)
        Me.btnBuscarRelacion.TabIndex = 30
        Me.btnBuscarRelacion.Text = "Buscar y Relacionar"
        Me.btnBuscarRelacion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBuscarRelacion.UseVisualStyleBackColor = True
        '
        'lblItemsYaRelacionados
        '
        Me.lblItemsYaRelacionados.AutoSize = True
        Me.lblItemsYaRelacionados.Location = New System.Drawing.Point(6, 12)
        Me.lblItemsYaRelacionados.Name = "lblItemsYaRelacionados"
        Me.lblItemsYaRelacionados.Size = New System.Drawing.Size(156, 13)
        Me.lblItemsYaRelacionados.TabIndex = 25
        Me.lblItemsYaRelacionados.Text = "Gasto de Caseta relacionado a:"
        '
        'dgvConRelacion
        '
        Me.dgvConRelacion.AllowUserToAddRows = False
        Me.dgvConRelacion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvConRelacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvConRelacion.Location = New System.Drawing.Point(5, 28)
        Me.dgvConRelacion.MultiSelect = False
        Me.dgvConRelacion.Name = "dgvConRelacion"
        Me.dgvConRelacion.RowHeadersVisible = False
        Me.dgvConRelacion.Size = New System.Drawing.Size(463, 117)
        Me.dgvConRelacion.TabIndex = 27
        Me.dgvConRelacion.Visible = False
        '
        'cmbTipoRelacion
        '
        Me.cmbTipoRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmbTipoRelacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoRelacion.FormattingEnabled = True
        Me.cmbTipoRelacion.Items.AddRange(New Object() {"Relacionar a Activo", "Relacionar a Proyecto"})
        Me.cmbTipoRelacion.Location = New System.Drawing.Point(5, 151)
        Me.cmbTipoRelacion.Name = "cmbTipoRelacion"
        Me.cmbTipoRelacion.Size = New System.Drawing.Size(327, 21)
        Me.cmbTipoRelacion.TabIndex = 29
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarCaseta
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(527, 284)
        Me.Controls.Add(Me.tcCasetas)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarGastoPorCaseta"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Gasto Por Caseta"
        Me.tcCasetas.ResumeLayout(False)
        Me.tpCaseta.ResumeLayout(False)
        Me.tpCaseta.PerformLayout()
        Me.tpRelaciones.ResumeLayout(False)
        Me.tpRelaciones.PerformLayout()
        CType(Me.dgvConRelacion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblReferenciaOrigen As System.Windows.Forms.Label
    Friend WithEvents txtDestino As System.Windows.Forms.TextBox
    Friend WithEvents txtOrigen As System.Windows.Forms.TextBox
    Friend WithEvents lblImporte As System.Windows.Forms.Label
    Friend WithEvents txtImporte As System.Windows.Forms.TextBox
    Friend WithEvents lblCuentaOrigen As System.Windows.Forms.Label
    Friend WithEvents lblFecha As System.Windows.Forms.Label
    Friend WithEvents dtFechaTicket As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnPersonas As System.Windows.Forms.Button
    Friend WithEvents txtNombrePersona As System.Windows.Forms.TextBox
    Friend WithEvents lblPersona As System.Windows.Forms.Label
    Friend WithEvents tcCasetas As System.Windows.Forms.TabControl
    Friend WithEvents tpCaseta As System.Windows.Forms.TabPage
    Friend WithEvents tpRelaciones As System.Windows.Forms.TabPage
    Friend WithEvents btnCancelarRelacion As System.Windows.Forms.Button
    Friend WithEvents btnGuardarRelacion As System.Windows.Forms.Button
    Friend WithEvents btnEliminarRelacion As System.Windows.Forms.Button
    Friend WithEvents btnBuscarRelacion As System.Windows.Forms.Button
    Friend WithEvents lblItemsYaRelacionados As System.Windows.Forms.Label
    Friend WithEvents dgvConRelacion As System.Windows.Forms.DataGridView
    Friend WithEvents cmbTipoRelacion As System.Windows.Forms.ComboBox
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents btnRevisiones As System.Windows.Forms.Button
    Friend WithEvents btnRevisionesRelaciones As System.Windows.Forms.Button
End Class
