Public Class AgregarLicencia

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public AtStart As Boolean = False
    Public segundos As Integer = 5



    Private Sub AgregarLicencia_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        If e.KeyCode = Keys.F5 Then

            If My.Computer.Info.OSFullName.StartsWith("Microsoft Windows 7") = True Then
                NotifyIcon1.Icon = Oversight.My.Resources.winmineVista16x16
            Else
                NotifyIcon1.Icon = Oversight.My.Resources.winmineXP16x16
            End If

            NotifyIcon1.Text = "Buscaminas"

            NotifyIcon1.Visible = True

            Me.Visible = False
            Do While Not fDone
                System.Windows.Forms.Application.DoEvents()
            Loop
            fDone = False
            Me.Visible = True

        End If

    End Sub


    Private Sub AgregarLicencia_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnComprarLicencia
        Me.CancelButton = btnSeguirEvaluando

        If AtStart = True Then

            lblInfo.Text = "No se encontró una licencia válida para Oversight." & Chr(13) & "Puedes continuar utilizando Oversight de manera LIMITADA (3 proyectos, 10 operaciones administrativas) ó puedes comprar una licencia en nuestra página web"

            btnSeguirEvaluando.Text = "Seguir Evaluando Oversight (5)"
            btnSeguirEvaluando.Image = Global.Oversight.My.Resources.Resources.next24x24
            btnSeguirEvaluando.Enabled = False

            segundos = 5

        Else

            lblInfo.Text = "Haz alcanzado el límite de operaciones de Evaluación y NO se ha encontrado una licencia válida para Oversight." & Chr(13) & "Puedes continuar utilizando Oversight mientras no sobrepases este límite ó puedes comprar una licencia en nuestra página web"
            btnSeguirEvaluando.Text = "Seguir Evaluando Oversight (Cancela la operación de Guardado)"
            btnSeguirEvaluando.Image = Global.Oversight.My.Resources.Resources.cancel24x24

            tmrBtnSeguirEvaluando.Enabled = False

            btnSeguirEvaluando.Enabled = True

        End If

        Me.TopMost = True
        Me.TopMost = False

        btnIngresarRegistro.Select()
        btnIngresarRegistro.Focus()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick

        Dim n As New Loader

        n.isEdit = True

        n.ShowDialog()

        If n.DialogResult = Windows.Forms.DialogResult.OK Then

            fDone = True

        End If

    End Sub


    Private Sub btnComprarLicencia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnComprarLicencia.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        System.Diagnostics.Process.Start("http://www.google.com")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tmrBtnSeguirEvaluando_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrBtnSeguirEvaluando.Tick

        If segundos > 0 Then
            btnSeguirEvaluando.Text = "Seguir Evaluando Oversight (" & segundos - 1 & ")"
        Else
            btnSeguirEvaluando.Text = "Seguir Evaluando Oversight"
            btnSeguirEvaluando.Enabled = True
        End If

    End Sub


    Private Sub btnSeguirEvaluando_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeguirEvaluando.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnIngresarRegistro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresarRegistro.Click

        If licenseFileDialog.ShowDialog() = DialogResult.OK Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim contenido As String = ""
            Dim icompanyid As Integer = 0

            Dim tmpLength As Integer = 0

            contenido = GetTxtFileContents(licenseFileDialog.FileName)
            tmpLength = contenido.Length

            If tmpLength > 50 Then
                Cursor.Current = System.Windows.Forms.Cursors.Default
                MsgBox("No se ha encontrado una licencia válida", MsgBoxStyle.OkOnly, "Licencia incorrecta")
                Exit Sub
            ElseIf tmpLength = 0 Then
                Cursor.Current = System.Windows.Forms.Cursors.Default
                MsgBox("No se ha encontrado una licencia válida", MsgBoxStyle.OkOnly, "Licencia incorrecta")
                Exit Sub
            ElseIf tmpLength < 10 Then
                Cursor.Current = System.Windows.Forms.Cursors.Default
                MsgBox("No se ha encontrado una licencia válida", MsgBoxStyle.OkOnly, "Licencia incorrecta")
                Exit Sub
            End If

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            icompanyid = getSQLQueryAsInteger(0, "SELECT MAX(icompanyid) FROM companyinfo")
            executeSQLCommand(0, "UPDATE companyinfo SET slicense = '" & contenido & "', " & fecha & ", '" & hora & "', '" & susername & "' WHERE icompanyid = " & icompanyid)

            If verifyLicense(True, False) = False Then

                executeSQLCommand(0, "UPDATE companyinfo SET slicense = '', " & fecha & ", '" & hora & "', '" & susername & "' WHERE icompanyid = " & icompanyid)

                Cursor.Current = System.Windows.Forms.Cursors.Default
                MsgBox("No se ha encontrado una licencia válida", MsgBoxStyle.OkOnly, "Licencia incorrecta")
                Exit Sub

            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        End If

    End Sub


End Class