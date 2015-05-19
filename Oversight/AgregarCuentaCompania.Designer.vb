<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarCuentaCompania
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarCuentaCompania))
        Me.gbCuenta = New System.Windows.Forms.GroupBox
        Me.btnBancos = New System.Windows.Forms.Button
        Me.cmbBancos = New System.Windows.Forms.ComboBox
        Me.lblBanco = New System.Windows.Forms.Label
        Me.lblDescripcion = New System.Windows.Forms.Label
        Me.txtDescripcion = New System.Windows.Forms.TextBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbCuenta.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbCuenta
        '
        Me.gbCuenta.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbCuenta.Controls.Add(Me.btnBancos)
        Me.gbCuenta.Controls.Add(Me.cmbBancos)
        Me.gbCuenta.Controls.Add(Me.lblBanco)
        Me.gbCuenta.Controls.Add(Me.lblDescripcion)
        Me.gbCuenta.Controls.Add(Me.txtDescripcion)
        Me.gbCuenta.Controls.Add(Me.btnGuardar)
        Me.gbCuenta.Controls.Add(Me.btnCancelar)
        Me.gbCuenta.Location = New System.Drawing.Point(3, -2)
        Me.gbCuenta.Name = "gbCuenta"
        Me.gbCuenta.Size = New System.Drawing.Size(451, 195)
        Me.gbCuenta.TabIndex = 57
        Me.gbCuenta.TabStop = False
        '
        'btnBancos
        '
        Me.btnBancos.Enabled = False
        Me.btnBancos.Image = Global.Oversight.My.Resources.Resources.bank12x12
        Me.btnBancos.Location = New System.Drawing.Point(319, 11)
        Me.btnBancos.Name = "btnBancos"
        Me.btnBancos.Size = New System.Drawing.Size(28, 23)
        Me.btnBancos.TabIndex = 75
        Me.btnBancos.UseVisualStyleBackColor = True
        '
        'cmbBancos
        '
        Me.cmbBancos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBancos.FormattingEnabled = True
        Me.cmbBancos.Location = New System.Drawing.Point(81, 13)
        Me.cmbBancos.Name = "cmbBancos"
        Me.cmbBancos.Size = New System.Drawing.Size(232, 21)
        Me.cmbBancos.TabIndex = 65
        '
        'lblBanco
        '
        Me.lblBanco.AutoSize = True
        Me.lblBanco.Location = New System.Drawing.Point(9, 16)
        Me.lblBanco.Name = "lblBanco"
        Me.lblBanco.Size = New System.Drawing.Size(41, 13)
        Me.lblBanco.TabIndex = 64
        Me.lblBanco.Text = "Banco:"
        '
        'lblDescripcion
        '
        Me.lblDescripcion.AutoSize = True
        Me.lblDescripcion.Location = New System.Drawing.Point(9, 47)
        Me.lblDescripcion.Name = "lblDescripcion"
        Me.lblDescripcion.Size = New System.Drawing.Size(66, 13)
        Me.lblDescripcion.TabIndex = 63
        Me.lblDescripcion.Text = "Descripción:"
        '
        'txtDescripcion
        '
        Me.txtDescripcion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescripcion.Location = New System.Drawing.Point(81, 45)
        Me.txtDescripcion.MaxLength = 500
        Me.txtDescripcion.Multiline = True
        Me.txtDescripcion.Name = "txtDescripcion"
        Me.txtDescripcion.Size = New System.Drawing.Size(362, 100)
        Me.txtDescripcion.TabIndex = 2
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.creditcard24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(319, 151)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(124, 34)
        Me.btnGuardar.TabIndex = 4
        Me.btnGuardar.Text = "&Guardar Cuenta"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(212, 151)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(101, 34)
        Me.btnCancelar.TabIndex = 3
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarCuentaCompania
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(458, 197)
        Me.Controls.Add(Me.gbCuenta)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarCuentaCompania"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Cuenta de la Compañía"
        Me.gbCuenta.ResumeLayout(False)
        Me.gbCuenta.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbCuenta As System.Windows.Forms.GroupBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblDescripcion As System.Windows.Forms.Label
    Friend WithEvents txtDescripcion As System.Windows.Forms.TextBox
    Friend WithEvents cmbBancos As System.Windows.Forms.ComboBox
    Friend WithEvents lblBanco As System.Windows.Forms.Label
    Friend WithEvents btnBancos As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
