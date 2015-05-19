<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BuscaInsumosPreguntaTipoNuevoInsumo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BuscaInsumosPreguntaTipoNuevoInsumo))
        Me.gbBuscar = New System.Windows.Forms.GroupBox
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.rbNormal = New System.Windows.Forms.RadioButton
        Me.rbCompuesto = New System.Windows.Forms.RadioButton
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
        Me.FlowLayoutPanel1.Controls.Add(Me.rbNormal)
        Me.FlowLayoutPanel1.Controls.Add(Me.rbCompuesto)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(11, 46)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(425, 73)
        Me.FlowLayoutPanel1.TabIndex = 41
        '
        'rbNormal
        '
        Me.rbNormal.AutoSize = True
        Me.rbNormal.Location = New System.Drawing.Point(3, 3)
        Me.rbNormal.MaximumSize = New System.Drawing.Size(450, 0)
        Me.rbNormal.Name = "rbNormal"
        Me.rbNormal.Size = New System.Drawing.Size(95, 17)
        Me.rbNormal.TabIndex = 34
        Me.rbNormal.TabStop = True
        Me.rbNormal.Text = "Insumo Normal"
        Me.rbNormal.UseVisualStyleBackColor = True
        '
        'rbCompuesto
        '
        Me.rbCompuesto.AutoSize = True
        Me.rbCompuesto.Location = New System.Drawing.Point(3, 26)
        Me.rbCompuesto.Name = "rbCompuesto"
        Me.rbCompuesto.Size = New System.Drawing.Size(415, 17)
        Me.rbCompuesto.TabIndex = 42
        Me.rbCompuesto.TabStop = True
        Me.rbCompuesto.Text = "Insumo Compuesto (Ej: como el Concreto, que usa otros insumos para su creación)"
        Me.rbCompuesto.UseVisualStyleBackColor = True
        '
        'lblIntro
        '
        Me.lblIntro.AutoSize = True
        Me.lblIntro.Location = New System.Drawing.Point(8, 16)
        Me.lblIntro.MaximumSize = New System.Drawing.Size(480, 0)
        Me.lblIntro.Name = "lblIntro"
        Me.lblIntro.Size = New System.Drawing.Size(214, 13)
        Me.lblIntro.TabIndex = 33
        Me.lblIntro.Text = "Escoge el tipo de Insumo que deseas crear:"
        '
        'btnActualizar
        '
        Me.btnActualizar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnActualizar.Image = Global.Oversight.My.Resources.Resources.bricks24x24
        Me.btnActualizar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnActualizar.Location = New System.Drawing.Point(292, 139)
        Me.btnActualizar.Name = "btnActualizar"
        Me.btnActualizar.Size = New System.Drawing.Size(144, 34)
        Me.btnActualizar.TabIndex = 29
        Me.btnActualizar.Text = "&Crear Nuevo Insumo!"
        Me.btnActualizar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnActualizar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(197, 139)
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
        'BuscaInsumosPreguntaTipoNuevoInsumo
        '
        Me.AcceptButton = Me.btnActualizar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(456, 185)
        Me.Controls.Add(Me.gbBuscar)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "BuscaInsumosPreguntaTipoNuevoInsumo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Nuevo Insumo - Escoger tipo de Insumo"
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
    Friend WithEvents rbNormal As System.Windows.Forms.RadioButton
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents rbCompuesto As System.Windows.Forms.RadioButton
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
