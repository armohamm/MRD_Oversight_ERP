Public Class Reportes

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public ireportid As Integer = 0

    Private dgvReporte As DataGridView
    Private WithEvents reportesTabPage As TabPage
    Private currentSelectedTabtcReportes As String = ""
    Private WithEvents btnExportarReporte As Button

    Private isFormReadyForAction As Boolean = False

    Private viewPermission As Boolean = False
    Private viewReports As Boolean = False

    Public messagesWindowIsAlreadyOpen As Boolean = False


    Private Sub checkMessages(ByVal username As String, ByVal x As Integer, ByVal y As Integer)

        Dim unreadmessagecount As Integer = 0
        unreadmessagecount = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM messages where susername = '" & username & "' AND bread = 0")

        If unreadmessagecount > 0 And messagesWindowIsAlreadyOpen = False Then

            Dim msg As New Mensajes
            Dim pt As Point

            msg.susername = username
            msg.bactive = bactive
            msg.bonline = bonline
            msg.suserfullname = suserfullname
            msg.suseremail = suseremail
            msg.susersession = susersession
            msg.susermachinename = susermachinename
            msg.suserip = suserip

            msg.StartPosition = FormStartPosition.Manual

            Dim tamañoPantalla As Integer = Screen.GetWorkingArea(Me).Height

            Dim tmpPt1 As Point = New Point(Me.Location.X, (tamañoPantalla - Me.Size.Height - msg.Size.Height) / 2) 'msg window
            Dim tmpPt2 As Point = New Point(Me.Location.X, tmpPt1.Y + msg.Size.Height) 'me

            If tmpPt1.Y > Screen.GetWorkingArea(Me).Location.Y Then

                pt = New Point(Me.Location.X, tmpPt1.Y)
                Me.Location = New Point(Me.Location.X, tmpPt2.Y)

            Else

                pt = New Point(x, y)

            End If

            msg.Location = pt
            msg.bAlreadyOpen = True

            messagesWindowIsAlreadyOpen = True

            msg.Show()

        End If

    End Sub


    Private Sub checkForKickoutsAndTimedOuts()

        Dim queryMySession As String = ""
        Dim dsMySession As DataSet

        queryMySession = "SELECT * FROM sessions s WHERE s.susername = '" & susername & "' AND s.susersession = '" & susersession & "' ORDER BY s.ilogindate DESC, s.slogintime DESC LIMIT 1 "

        dsMySession = getSQLQueryAsDataset(0, queryMySession)

        If dsMySession.Tables(0).Rows.Count > 0 Then

            If dsMySession.Tables(0).Rows(0).Item("btimedout") = "1" Then

                MsgBox("Tu sesión ha expirado. Es necesario que entres de nuevo al sistema con tu usuario y contraseña", MsgBoxStyle.Critical, "Sesión expirada")

                susername = ""
                bactive = False
                bonline = False
                suserfullname = ""
                suseremail = ""
                susersession = 0
                susermachinename = ""
                suserip = "0.0.0.0"

                Dim l As New Login

                l.isEdit = True

                l.ShowDialog(Me)

                If l.DialogResult <> Windows.Forms.DialogResult.OK Then

                    MsgBox("Cerrando Aplicación SIN Guardar...", MsgBoxStyle.Critical, "Intento Fallido")
                    System.Environment.Exit(0)

                End If

            End If

            If dsMySession.Tables(0).Rows(0).Item("bkickedout") = "1" Then

                MsgBox("Has sido sacado del sistema. Para continuar es necesario que entres de nuevo al sistema con tu usuario y contraseña", MsgBoxStyle.Critical, "Logged out")

                susername = ""
                bactive = False
                bonline = False
                suserfullname = ""
                suseremail = ""
                susersession = 0
                susermachinename = ""
                suserip = "0.0.0.0"

                Dim l As New Login

                l.isEdit = True

                l.ShowDialog(Me)

                If l.DialogResult <> Windows.Forms.DialogResult.OK Then

                    MsgBox("Cerrando Aplicación SIN Guardar...", MsgBoxStyle.Critical, "Intento Fallido")
                    System.Environment.Exit(0)

                End If

            End If

        End If

    End Sub


    Private Sub setControlsByPermissions(ByVal windowname As String, ByVal username As String)

        'Check for specific permissions on every window, but only for that unique window permissions, not the entire list!!

        Dim dsPermissions As DataSet

        Dim permission As String

        dsPermissions = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions WHERE susername = '" & username & "' AND swindowname = '" & windowname & "'")

        For j = 0 To dsPermissions.Tables(0).Rows.Count - 1

            Try

                permission = dsPermissions.Tables(0).Rows(j).Item("spermission")

                If permission = "Ver" Then
                    viewPermission = True
                End If

                If permission = "Modificar" Then
                    'Nothing
                End If

                If permission = "Generar Archivo Excel" Then
                    'btnGenerarArchivoExcel.Enabled = True
                End If

                If permission = "Ver Reportes" Then
                    viewReports = True
                End If

            Catch ex As Exception

            End Try

            permission = ""

        Next j


        If viewPermission = False Then

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Proyecto', 'OK')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedatetime, smessagecreatorusername, iupdatedatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Un usuario ha intentado propasar sus permisos. ¿Podrías revisar?', 0, '" & convertYYYYMMDDtoYYYYhyphenMMhyphenDD(fecha) & " " & hora & "', 'SYSTEM', '" & convertYYYYMMDDtoYYYYhyphenMMhyphenDD(fecha) & " " & hora & "', 'SYSTEM')")
                Next i

            End If

            MsgBox("No tienes los permisos necesarios para abrir esta Ventana. Este intento ha sido notificado al administrador.", MsgBoxStyle.Exclamation, "Access Denied")

            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()

        End If

    End Sub


    Private Sub Reportes_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la Ventana de Reportes " & ireportid & "', 'OK')")

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub Reportes_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub Reportes_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        If tcVentana.SelectedTab.Text = "Reportes" Then

            'If viewReports = False Then
            '    dgvReporte.Visible = False
            'End If

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim dsReportes As DataSet
            dsReportes = getSQLQueryAsDataset(0, "SELECT * FROM reports")

            If dsReportes.Tables(0).Rows.Count < 1 Then
                Exit Sub
            End If

            tcReportes.TabPages.Clear()

            For i = 0 To dsReportes.Tables(0).Rows.Count - 1

                If dsReportes.Tables(0).Rows(i).Item("sreportquery") = "" Then
                    Continue For
                End If

                Dim tb As New TabPage
                tb.Name = dsReportes.Tables(0).Rows(i).Item("stabid")
                tb.Text = dsReportes.Tables(0).Rows(i).Item("stabname")
                tb.Dock = DockStyle.None
                tb.Location = New Point(0, 24)

                dgvReporte = New DataGridView
                btnExportarReporte = New Button

                dgvReporte.Location = New Drawing.Point(5, 5)

                dgvReporte.Height = tcReportes.Height - 40
                dgvReporte.Width = tcReportes.Width - 48

                btnExportarReporte.Enabled = True
                btnExportarReporte.Visible = True
                btnExportarReporte.Image = Global.Oversight.My.Resources.Resources.excel12x12
                btnExportarReporte.Location = New System.Drawing.Point(5 + dgvReporte.Width + 4, 5)
                btnExportarReporte.Name = "btnExportarReporte"
                btnExportarReporte.Size = New System.Drawing.Size(28, 23)
                btnExportarReporte.TabIndex = 0
                btnExportarReporte.TabStop = False
                btnExportarReporte.UseVisualStyleBackColor = True
                btnExportarReporte.Text = ""

                tb.Controls.Add(Me.btnExportarReporte)

                Dim dsReportesColumnsSizes As DataSet
                dsReportesColumnsSizes = getSQLQueryAsDataset(0, "SELECT icolumnid, columnwidth FROM reportcolumnssizes WHERE ireportid = " & ireportid)

                executeSQLCommand(0, dsReportes.Tables(0).Rows(i).Item("sreportbeforequery"))

                setDataGridView(dgvReporte, dsReportes.Tables(0).Rows(i).Item("sreportquery"), True)

                executeSQLCommand(0, dsReportes.Tables(0).Rows(i).Item("sreportafterquery"))

                dgvReporte.AllowUserToAddRows = False
                dgvReporte.AllowUserToDeleteRows = False
                dgvReporte.MultiSelect = False
                dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                dgvReporte.RowHeadersVisible = False
                dgvReporte.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.None)

                Dim alto As Integer = 20

                Try
                    alto = dsReportes.Tables(0).Rows(i).Item("rowheight")
                Catch ex As Exception

                End Try

                dgvReporte.ColumnHeadersHeight = alto

                tb.Controls.Add(dgvReporte)

                tcReportes.TabPages.Add(tb)

                If dsReportesColumnsSizes.Tables(0).Rows.Count > 0 Then

                    For j = 0 To dsReportesColumnsSizes.Tables(0).Rows.Count - 1

                        Dim columna As Integer = 0
                        Dim ancho As Integer = 70

                        Try
                            columna = dsReportesColumnsSizes.Tables(0).Rows(j).Item("icolumnid")
                            ancho = dsReportesColumnsSizes.Tables(0).Rows(j).Item("columnwidth")
                        Catch ex As Exception

                        End Try

                        dgvReporte.Columns(j).Width = ancho

                    Next j

                End If

                dsReportesColumnsSizes.Clear()
                dsReportesColumnsSizes.Dispose()

            Next i

            currentSelectedTabtcReportes = dsReportes.Tables(0).Rows(0).Item("stabname")

        End If

    End Sub


    Private Sub Reportes_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        loadProgramSettings()


        'If viewReports = False Then
        '    dgvReporte.Visible = False
        'End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim dsReportes As DataSet
        dsReportes = getSQLQueryAsDataset(0, "SELECT * FROM reports")

        If dsReportes.Tables(0).Rows.Count < 1 Then
            Exit Sub
        End If

        tcReportes.TabPages.Clear()

        For i = 0 To dsReportes.Tables(0).Rows.Count - 1

            If dsReportes.Tables(0).Rows(i).Item("sreportquery") = "" Then
                Continue For
            End If

            Dim tb As New TabPage
            tb.Name = dsReportes.Tables(0).Rows(i).Item("stabid")
            tb.Text = dsReportes.Tables(0).Rows(i).Item("stabname")
            tb.Dock = DockStyle.None
            tb.Location = New Point(0, 24)

            dgvReporte = New DataGridView
            btnExportarReporte = New Button

            dgvReporte.Location = New Drawing.Point(5, 5)

            dgvReporte.Height = tcReportes.Height - 40
            dgvReporte.Width = tcReportes.Width - 48

            btnExportarReporte.Enabled = True
            btnExportarReporte.Visible = True
            btnExportarReporte.Image = Global.Oversight.My.Resources.Resources.excel12x12
            btnExportarReporte.Location = New System.Drawing.Point(5 + dgvReporte.Width + 4, 5)
            btnExportarReporte.Name = "btnExportarReporte"
            btnExportarReporte.Size = New System.Drawing.Size(28, 23)
            btnExportarReporte.TabIndex = 0
            btnExportarReporte.TabStop = False
            btnExportarReporte.UseVisualStyleBackColor = True
            btnExportarReporte.Text = ""

            tb.Controls.Add(Me.btnExportarReporte)

            Dim dsReportesColumnsSizes As DataSet
            dsReportesColumnsSizes = getSQLQueryAsDataset(0, "SELECT icolumnid, columnwidth FROM reportcolumnssizes WHERE ireportid = " & ireportid)

            executeSQLCommand(0, dsReportes.Tables(0).Rows(i).Item("sreportbeforequery"))

            setDataGridView(dgvReporte, dsReportes.Tables(0).Rows(i).Item("sreportquery"), True)

            executeSQLCommand(0, dsReportes.Tables(0).Rows(i).Item("sreportafterquery"))

            dgvReporte.AllowUserToAddRows = False
            dgvReporte.AllowUserToDeleteRows = False
            dgvReporte.MultiSelect = False
            dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvReporte.RowHeadersVisible = False
            dgvReporte.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.None)

            Dim alto As Integer = 20

            Try
                alto = dsReportes.Tables(0).Rows(i).Item("rowheight")
            Catch ex As Exception

            End Try

            dgvReporte.ColumnHeadersHeight = alto

            tb.Controls.Add(dgvReporte)

            tcReportes.TabPages.Add(tb)

            If dsReportesColumnsSizes.Tables(0).Rows.Count > 0 Then

                For j = 0 To dsReportesColumnsSizes.Tables(0).Rows.Count - 1

                    Dim columna As Integer = 0
                    Dim ancho As Integer = 70

                    Try
                        columna = dsReportesColumnsSizes.Tables(0).Rows(j).Item("icolumnid")
                        ancho = dsReportesColumnsSizes.Tables(0).Rows(j).Item("columnwidth")
                    Catch ex As Exception

                    End Try

                    dgvReporte.Columns(j).Width = ancho

                Next j

            End If

            dsReportesColumnsSizes.Clear()
            dsReportesColumnsSizes.Dispose()

        Next i

        currentSelectedTabtcReportes = dsReportes.Tables(0).Rows(0).Item("stabname")


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


    Private Sub tcReportes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcReportes.SelectedIndexChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim dsReportes As DataSet
        dsReportes = getSQLQueryAsDataset(0, "SELECT * FROM reports")

        If dsReportes.Tables(0).Rows.Count < 1 Then
            Exit Sub
        End If

        If tcReportes.SelectedIndex = -1 Then
            currentSelectedTabtcReportes = dsReportes.Tables(0).Rows(0).Item("stabname")
        Else
            currentSelectedTabtcReportes = dsReportes.Tables(0).Rows(tcReportes.SelectedIndex).Item("stabname")
        End If

        'For i = 0 To dsReportes.Tables(0).Rows.Count - 1

        '    If dsReportes.Tables(0).Rows(i).Item("sreportquery") = "" Then
        '        Continue For
        '    End If

        '    Dim dsReportesColumnsSizes As DataSet
        '    dsReportesColumnsSizes = getSQLQueryAsDataset(0, "SELECT icolumnid, columnwidth FROM reportcolumnssizes WHERE ireportid = " & ireportid & " AND ireportid = " & dsReportes.Tables(0).Rows(i).Item("ireportid"))

        '    executeSQLCommand(0, dsReportes.Tables(0).Rows(i).Item("sreportbeforequery"))

        '    setDataGridView(dgvReporte, dsReportes.Tables(0).Rows(i).Item("sreportquery"), True)

        '    executeSQLCommand(0, dsReportes.Tables(0).Rows(i).Item("sreportafterquery"))

        '    dgvReporte.AllowUserToAddRows = False
        '    dgvReporte.AllowUserToDeleteRows = False
        '    dgvReporte.MultiSelect = False
        '    dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        '    dgvReporte.RowHeadersVisible = False

        '    Dim alto As Integer = 20

        '    Try
        '        alto = dsReportes.Tables(0).Rows(i).Item("rowheight")
        '    Catch ex As Exception

        '    End Try

        '    dgvReporte.ColumnHeadersHeight = alto

        '    If dsReportesColumnsSizes.Tables(0).Rows.Count > 0 Then

        '        For j = 0 To dsReportesColumnsSizes.Tables(0).Rows.Count - 1

        '            Dim columna As Integer = 0
        '            Dim ancho As Integer = 70

        '            Try
        '                columna = dsReportesColumnsSizes.Tables(0).Rows(j).Item("icolumnid")
        '                ancho = dsReportesColumnsSizes.Tables(0).Rows(j).Item("columnwidth")
        '                dgvReporte.Columns(j).Width = ancho
        '            Catch ex As Exception

        '            End Try

        '        Next j

        '    End If

        '    dsReportesColumnsSizes.Clear()
        '    dsReportesColumnsSizes.Dispose()

        'Next i


    End Sub


    Private Sub btnGenerarExplosion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Dim queryPrecioSubTotal As String
        'Dim precioSubTotal As Double = 0.0
        'queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
        '"FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "Cards ptf " & _
        '"JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
        '"JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & " p ON p.ireportid = ptf.ireportid " & _
        '"JOIN (SELECT ptfi.ireportid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "Cards ptf ON ptf.ireportid = ptfi.ireportid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.ireportid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "CardInputs ptfi ON ptf.ireportid = ptfi.ireportid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, ireportid) pp ON ptfi.ireportid = pp.ireportid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.ireportid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.ireportid) AS costoEQ ON ptf.ireportid = costoEQ.ireportid AND ptf.icardid = costoEQ.icardid " & _
        '"JOIN (SELECT ptfi.ireportid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "CardInputs ptfi ON ptf.ireportid = ptfi.ireportid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, ireportid) pp ON ptfi.ireportid = pp.ireportid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.ireportid) AS costoMO ON ptf.ireportid = costoMO.ireportid AND ptf.icardid = costoMO.icardid " & _
        '"LEFT JOIN (SELECT ptfi.ireportid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "CardInputs ptfi ON ptf.ireportid = ptfi.ireportid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, ireportid) pp ON ptfi.ireportid = pp.ireportid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.ireportid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "CardCompoundInputs ptfci ON ptfci.ireportid = ptfi.ireportid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & ireportid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, ireportid) cipp ON cipp.ireportid = ptfci.ireportid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.ireportid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.ireportid = cipp.ireportid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.ireportid, ptfi.icardid ORDER BY ptfi.ireportid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.ireportid = costoMAT.ireportid AND ptf.icardid = costoMAT.icardid " & _
        '"WHERE p.ireportid = " & ireportid

        'precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

        'If (precioSubTotal = 0.0) Then
        '    MsgBox("Aún NO has terminado de definir este proyecto. ¿Podrías completarlo?", MsgBoxStyle.OkOnly, "Proyecto incompleto")
        '    Exit Sub
        'End If

        Try

            Dim resultado As Boolean = False

            Dim fecha As String = ""
            Dim dayAux As String = ""
            Dim monthAux As String = ""
            Dim hourAux As String = ""
            Dim minuteAux As String = ""
            Dim secondAux As String = ""

            If DateTime.Today.Month.ToString.Length < 2 Then
                monthAux = "0" & DateTime.Today.Month
            Else
                monthAux = DateTime.Today.Month
            End If

            If DateTime.Today.Day.ToString.Length < 2 Then
                dayAux = "0" & DateTime.Today.Day
            Else
                dayAux = DateTime.Today.Day
            End If

            If Date.Now.Hour.ToString.Length < 2 Then
                hourAux = "0" & DateTime.Now.Hour
            Else
                hourAux = DateTime.Now.Hour
            End If

            If Date.Now.Minute.ToString.Length < 2 Then
                minuteAux = "0" & DateTime.Now.Minute
            Else
                minuteAux = DateTime.Now.Minute
            End If

            If Date.Now.Second.ToString.Length < 2 Then
                secondAux = "0" & DateTime.Now.Second
            Else
                secondAux = DateTime.Now.Second
            End If

            fecha = DateTime.Today.Year & monthAux & dayAux & hourAux & minuteAux & secondAux



            msSaveFileDialog.FileName = "Explosión de Insumos " & " " & fecha

            'Try

            '    If Not My.Computer.FileSystem.DirectoryExists(txtRuta.Text) Then
            '        My.Computer.FileSystem.CreateDirectory(txtRuta.Text)
            '    End If

            '    msSaveFileDialog.InitialDirectory = txtRuta.Text

            'Catch ex As Exception

            'End Try

            msSaveFileDialog.Filter = "Excel Files (*.xls) |*.xls"
            msSaveFileDialog.DefaultExt = "*.xls"

            If msSaveFileDialog.ShowDialog() = DialogResult.OK Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                resultado = ExportExplosionToExcel(msSaveFileDialog.FileName)

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If resultado = True Then
                    MsgBox("Explosión de Insumos Exportada Correctamente!" & Chr(13) & "El archivo se abrirá al dar click en OK", MsgBoxStyle.OkOnly, "Exportación Completada")
                    System.Diagnostics.Process.Start(msSaveFileDialog.FileName)
                Else
                    MsgBox("No se ha podido exportar la Explosión de Insumos. Intente nuevamente.", MsgBoxStyle.OkOnly, "Error al exportar la Explosión de Insumos")
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Function ExportExplosionToExcel(ByVal pth As String) As Boolean

        Try

            Dim fs As New IO.StreamWriter(pth, False)
            fs.WriteLine("<?xml version=""1.0""?>")
            fs.WriteLine("<?mso-application progid=""Excel.Sheet""?>")
            fs.WriteLine("<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:x=""urn:schemas-microsoft-com:office:excel"" xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet"" xmlns:html=""http://www.w3.org/TR/REC-html40"">")

            ' Create the styles for the worksheet
            fs.WriteLine("  <Styles>")

            ' Style for the document name
            fs.WriteLine("  <Style ss:ID=""1"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("  <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""12"" ss:Bold=""1""></Font>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("   <Protection></Protection>")
            fs.WriteLine("  </Style>")

            ' Style for the column headers
            fs.WriteLine("   <Style ss:ID=""2"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("  <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""9"" ss:Bold=""1""></Font>")
            fs.WriteLine("  </Style>")


            ' Style for the left sided info
            fs.WriteLine("    <Style ss:ID=""9"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the right sided info
            fs.WriteLine("    <Style ss:ID=""10"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the middle sided info
            fs.WriteLine("    <Style ss:ID=""11"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the SUBtotals labels
            fs.WriteLine("    <Style ss:ID=""12"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FFCC00"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals labels
            fs.WriteLine("    <Style ss:ID=""13"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals
            fs.WriteLine("    <Style ss:ID=""14"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat ss:Format=""Standard""></NumberFormat>")
            fs.WriteLine("    </Style>")

            fs.WriteLine("  </Styles>")

            ' Write the worksheet contents
            fs.WriteLine("<Worksheet ss:Name=""Hoja1"">")
            fs.WriteLine("  <Table ss:DefaultColumnWidth=""60"" ss:DefaultRowHeight=""15"">")

            'Write the project header info
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""494.25""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""63""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""65.25"" ss:Span=""5""/>")

            fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
            fs.WriteLine("  <Cell ss:MergeAcross=""7"" ss:StyleID=""1""><Data ss:Type=""String"">EXPLOSION DE INSUMOS</Data></Cell>")
            fs.WriteLine("   </Row>")

            fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
            'fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", lblNombreDelProyecto.Text.Trim))
            'fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", txtNombreProyecto.Text.Trim))
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            'fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", lblNombreCliente.Text.Trim))
            'fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", txtNombreCliente.Text.Trim))
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", "Fecha:"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate()) & " " & getAppTime()))
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")


            'Write the grid headers columns (taken out since columns are already defined)
            'For Each col As DataGridViewColumn In dgv.Columns
            '    If col.Visible Then
            '        fs.WriteLine(String.Format("    <Column ss:Width=""{0}""></Column>", col.Width))
            '    End If
            'Next



            'Write the grid headers
            fs.WriteLine("    <Row ss:AutoFitHeight=""0"" ss:Height=""45"">")

            For Each col As DataGridViewColumn In dgvReporte.Columns
                If col.Visible Then
                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""2""><Data ss:Type=""String"">{0}</Data></Cell>", col.HeaderText))
                End If
            Next


            fs.WriteLine("    </Row>")

            ' Write contents for each cell
            For Each row As DataGridViewRow In dgvReporte.Rows

                If dgvReporte.AllowUserToAddRows = True And row.Index = dgvReporte.Rows.Count - 1 Then
                    Exit For
                End If

                Try

                    'If CDbl(row.Cells(10).Value.ToString) > 0 Then
                    fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
                    'Else
                    '   Continue For
                    'End If

                Catch ex As Exception
                    fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
                End Try

                For Each col As DataGridViewColumn In dgvReporte.Columns

                    If col.Visible Then

                        If row.Cells(col.Name).Value.ToString = "" Then

                            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""></Cell>"))

                        Else

                            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))

                        End If

                    End If

                Next col

                fs.WriteLine("    </Row>")

            Next row

            ' Close up the document
            fs.WriteLine("  </Table>")

            fs.WriteLine("  <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">")
            fs.WriteLine("   <PageSetup>")
            fs.WriteLine("  <Layout x:Orientation=""Landscape""/>")
            fs.WriteLine("  <Header x:Margin=""0.51181102362204722""/>")
            fs.WriteLine("  <Footer x:Margin=""0.51181102362204722""/>")
            fs.WriteLine("  <PageMargins x:Bottom=""0.98425196850393704"" x:Left=""0.74803149606299213"" x:Right=""0.74803149606299213"" x:Top=""0.98425196850393704""/>")
            fs.WriteLine("   </PageSetup>")
            fs.WriteLine("   <Unsynced/>")
            fs.WriteLine("   <Print>")
            fs.WriteLine("  <ValidPrinterInfo/>")
            fs.WriteLine("  <PaperSizeIndex>9</PaperSizeIndex>")
            fs.WriteLine("  <Scale>65</Scale>")
            fs.WriteLine("  <HorizontalResolution>200</HorizontalResolution>")
            fs.WriteLine("  <VerticalResolution>200</VerticalResolution>")
            fs.WriteLine("   </Print>")
            fs.WriteLine("   <Zoom>75</Zoom>")
            fs.WriteLine("   <Selected/>")
            fs.WriteLine("   <Panes>")
            fs.WriteLine("  <Pane>")
            fs.WriteLine("   <Number>3</Number>")
            fs.WriteLine("   <ActiveRow>16</ActiveRow>")
            fs.WriteLine("   <ActiveCol>7</ActiveCol>")
            fs.WriteLine("  </Pane>")
            fs.WriteLine("   </Panes>")
            fs.WriteLine("   <ProtectObjects>False</ProtectObjects>")
            fs.WriteLine("   <ProtectScenarios>False</ProtectScenarios>")
            fs.WriteLine("  </WorksheetOptions>")


            fs.WriteLine("</Worksheet>")
            fs.WriteLine("</Workbook>")
            fs.Close()

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    'Private Sub btnExportarReporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportarReporte.Click

    '    If currentSelectedTabtcReportes = "Comparación Presupuestado vs Utilizado" Then

    '        MsgBox("Hola")

    '    End If

    'End Sub

    'Private Sub btnExportarReporte_Click() Handles btnExportarReporte.Click

    '    If currentSelectedTabtcReportes = "Comparación Presupuestado vs Utilizado" Then

    '        MsgBox("Hola2")

    '    End If

    'End Sub
    

End Class