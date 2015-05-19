<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BuscaEnDirectorioPreguntaNuevo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BuscaEnDirectorioPreguntaNuevo))
        Me.gbBuscar = New System.Windows.Forms.GroupBox
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.rbPersona = New System.Windows.Forms.RadioButton
        Me.rbProveedor = New System.Windows.Forms.RadioButton
        Me.lblIntro = New System.Windows.Forms.Label
        Me.btnActualizar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbBuscar.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbBuscar
        '
        Me.gbBuscar.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbBuscar.Controls.Add(Me.FlowLayoutPanel1)
        Me.gbBuscar.Controls.Add(Me.lblIntro)
        Me.gbBuscar.Controls.Add(Me.btnActualizar)
        Me.gbBuscar.Controls.Add(Me.btnCancelar)
        Me.gbBuscar.Location = New System.Drawing.Point(4, 0)
        Me.gbBuscar.Name = "gbBuscar"
        Me.gbBuscar.Size = New System.Drawing.Size(447, 182)
        Me.gbBuscar.TabIndex = 29
        Me.gbBuscar.TabStop = False
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.Controls.Add(Me.rbPersona)
        Me.FlowLayoutPanel1.Controls.Add(Me.rbProveedor)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(11, 46)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(121, 73)
        Me.FlowLayoutPanel1.TabIndex = 41
        '
        'rbPersona
        '
        Me.rbPersona.AutoSize = True
        Me.rbPersona.Location = New System.Drawing.Point(3, 3)
        Me.rbPersona.MaximumSize = New System.Drawing.Size(450, 0)
        Me.rbPersona.Name = "rbPersona"
        Me.rbPersona.Size = New System.Drawing.Size(99, 17)
        Me.rbPersona.TabIndex = 34
        Me.rbPersona.TabStop = True
        Me.rbPersona.Text = "Nueva Persona"
        Me.rbPersona.UseVisualStyleBackColor = True
        '
        'rbProveedor
        '
        Me.rbProveedor.AutoSize = True
        Me.rbProveedor.Location = New System.Drawing.Point(3, 26)
        Me.rbProveedor.Name = "rbProveedor"
        Me.rbProveedor.Size = New System.Drawing.Size(109, 17)
        Me.rbProveedor.TabIndex = 42
        Me.rbProveedor.TabStop = True
        Me.rbProveedor.Text = "Nuevo Proveedor"
        Me.rbProveedor.UseVisualStyleBackColor = True
        '
        'lblIntro
        '
        Me.lblIntro.AutoSize = True
        Me.lblIntro.Location = New System.Drawing.Point(8, 16)
        Me.lblIntro.MaximumSize = New System.Drawing.Size(480, 0)
        Me.lblIntro.Name = "lblIntro"
        Me.lblIntro.Size = New System.Drawing.Size(341, 13)
        Me.lblIntro.TabIndex = 33
        Me.lblIntro.Text = "Escoge si deseas agregar una Nueva Persona o un Nuevo Proveedor:"
        '
        'btnActualizar
        '
        Me.btnActualizar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnActualizar.Image = Global.Oversight.My.Resources.Resources.new24x24
        Me.btnActualizar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnActualizar.Location = New System.Drawing.Point(351, 139)
        Me.btnActualizar.Name = "btnActualizar"
        Me.btnActualizar.Size = New System.Drawing.Size(89, 34)
        Me.btnActualizar.TabIndex = 29
        Me.btnActualizar.Text = "&Crear!"
        Me.btnActualizar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnActualizar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(256, 139)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelar.TabIndex = 30
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'BuscaEnDirectorioPreguntaNuevo
        '
        Me.AcceptButton = Me.btnActualizar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(456, 185)
        Me.Controls.Add(Me.gbBuscar)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "BuscaEnDirectorioPreguntaNuevo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Nueva Persona o Nuevo Proveedor"
        Me.gbBuscar.ResumeLayout(False)
        Me.gbBuscar.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbBuscar As System.Windows.Forms.GroupBox
    Friend WithEvents btnActualizar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblIntro As System.Windows.Forms.Label
    Friend WithEvents rbPersona As System.Windows.Forms.RadioButton
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents rbProveedor As System.Windows.Forms.RadioButton
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
