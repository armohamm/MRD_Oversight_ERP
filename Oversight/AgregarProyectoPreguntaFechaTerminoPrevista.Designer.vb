<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarProyectoPreguntaFechaTerminoPrevista
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarProyectoPreguntaFechaTerminoPrevista))
        Me.gbFechaTermino = New System.Windows.Forms.GroupBox
        Me.dtFechaTerminoPrevista = New System.Windows.Forms.DateTimePicker
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.lblFechaPrevistaTerminacion = New System.Windows.Forms.Label
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbFechaTermino.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbFechaTermino
        '
        Me.gbFechaTermino.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbFechaTermino.Controls.Add(Me.dtFechaTerminoPrevista)
        Me.gbFechaTermino.Controls.Add(Me.btnGuardar)
        Me.gbFechaTermino.Controls.Add(Me.lblFechaPrevistaTerminacion)
        Me.gbFechaTermino.Location = New System.Drawing.Point(5, 0)
        Me.gbFechaTermino.Name = "gbFechaTermino"
        Me.gbFechaTermino.Size = New System.Drawing.Size(398, 104)
        Me.gbFechaTermino.TabIndex = 1
        Me.gbFechaTermino.TabStop = False
        '
        'dtFechaTerminoPrevista
        '
        Me.dtFechaTerminoPrevista.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtFechaTerminoPrevista.Location = New System.Drawing.Point(282, 19)
        Me.dtFechaTerminoPrevista.Name = "dtFechaTerminoPrevista"
        Me.dtFechaTerminoPrevista.Size = New System.Drawing.Size(100, 20)
        Me.dtFechaTerminoPrevista.TabIndex = 61
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.save24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(272, 56)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(119, 34)
        Me.btnGuardar.TabIndex = 57
        Me.btnGuardar.Text = "&Guardar Fecha"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'lblFechaPrevistaTerminacion
        '
        Me.lblFechaPrevistaTerminacion.AutoSize = True
        Me.lblFechaPrevistaTerminacion.Location = New System.Drawing.Point(7, 19)
        Me.lblFechaPrevistaTerminacion.Name = "lblFechaPrevistaTerminacion"
        Me.lblFechaPrevistaTerminacion.Size = New System.Drawing.Size(269, 13)
        Me.lblFechaPrevistaTerminacion.TabIndex = 23
        Me.lblFechaPrevistaTerminacion.Text = "Fecha Prometida/Prevista de Terminación de Proyecto:"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarProyectoPreguntaFechaTerminoPrevista
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(408, 110)
        Me.Controls.Add(Me.gbFechaTermino)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "AgregarProyectoPreguntaFechaTerminoPrevista"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Fecha de Término"
        Me.gbFechaTermino.ResumeLayout(False)
        Me.gbFechaTermino.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbFechaTermino As System.Windows.Forms.GroupBox
    Friend WithEvents lblFechaPrevistaTerminacion As System.Windows.Forms.Label
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents dtFechaTerminoPrevista As System.Windows.Forms.DateTimePicker
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
