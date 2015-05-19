Public Class BuscaInsumos

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public querystring As String = ""

    Public iprojectid As Integer = 0
    Public icardid As Integer = 0
    Public iinputid As Integer = 0

    Public sinputunit As String = ""
    Public sinputdescription As String = ""

    Public IsEdit As Boolean = False

    Public IsBase As Boolean = False
    Public IsModel As Boolean = False
    Public IsHistoric As Boolean = False

    Public restrictCompounds As Boolean = False
    Public mayNeedBaseTablesCreation As Boolean = False

    Public wasCreated As Boolean = False

    Private openPermission As Boolean = False

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

        Dim viewPermission As Boolean = False

        dsPermissions = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions WHERE susername = '" & username & "' AND swindowname = '" & windowname & "'")

        For j = 0 To dsPermissions.Tables(0).Rows.Count - 1

            Try

                permission = dsPermissions.Tables(0).Rows(j).Item("spermission")

                If permission = "Ver" Then
                    viewPermission = True
                End If

                If permission = "Nuevo" Then
                    btnCrear.Enabled = True
                End If

                If permission = "Modificar" Then
                    openPermission = True
                    btnAbrir.Enabled = True
                End If

                If permission = "Exportar" Then
                    btnExportar.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Buscar Insumos', 'OK')")

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


    Private Sub BuscaInsumos_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la ventana de Buscar Insumos con el Insumo " & iinputid & " : " & sinputdescription & " seleccionado', 'OK')")

    End Sub


    Private Sub BuscaInsumos_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub BuscaInsumos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnAbrir
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        txtBuscar.Text = querystring

        querystring = querystring.Replace(" ", "%")

        Dim queryBusqueda As String

        If IsBase = True Then

            If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE 'tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "%Base" & iprojectid & "'") > 0 Then

                queryBusqueda = "" & _
                "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                "FORMAT(IF(bp.dinputfinalprice IS NULL, IF(cib.dinputfinalprice IS NULL, IF(cib2.dinputfinalprice IS NULL, 0, cib2.dinputfinalprice), cib.dinputfinalprice), bp.dinputfinalprice), 2) AS 'Precio', " & _
                "IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS 'Categoria' " & _
                "FROM inputs i " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT btfci.ibaseid, btfci.icardid, btfci.iinputid, SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS dinputfinalprice FROM basecardcompoundinputs btfci LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid ORDER BY bp.iupdatedate DESC, bp.supdatetime DESC) cib GROUP BY ibaseid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cib ON i.iinputid = cib.iinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT btfci.ibaseid, btfci.icardid, btfci.iinputid, SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid ORDER BY bp.iupdatedate DESC, bp.supdatetime DESC) cib2 GROUP BY ibaseid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cib2 ON i.iinputid = cib2.iinputid " & _
                "LEFT JOIN inputcategories ic ON i.iinputid = ic.iinputid " & _
                "WHERE i.sinputdescription LIKE '%" & querystring & "%' OR i.sinputunit LIKE '%" & querystring & "%' OR ic.sinputcategory LIKE '%" & querystring & "%' " & _
                "GROUP BY i.iinputid " & _
                "ORDER BY i.sinputdescription "

            Else

                queryBusqueda = "" & _
                "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                "FORMAT(IF(bp.dinputfinalprice IS NULL, IF(cib.dinputfinalprice IS NULL, 0, cib.dinputfinalprice), bp.dinputfinalprice), 2) AS 'Precio', " & _
                "IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS 'Categoria' " & _
                "FROM inputs i " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT btfci.ibaseid, btfci.icardid, btfci.iinputid, SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS dinputfinalprice FROM basecardcompoundinputs btfci LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid ORDER BY bp.iupdatedate DESC, bp.supdatetime DESC) cib GROUP BY ibaseid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cib ON i.iinputid = cib.iinputid " & _
                "LEFT JOIN inputcategories ic ON i.iinputid = ic.iinputid " & _
                "WHERE i.sinputdescription LIKE '%" & querystring & "%' OR i.sinputunit LIKE '%" & querystring & "%' OR ic.sinputcategory LIKE '%" & querystring & "%' " & _
                "GROUP BY i.iinputid " & _
                "ORDER BY i.sinputdescription "

            End If

        Else

            If IsModel = True Then

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE 'tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "%Model" & iprojectid & "'") > 0 Then

                    queryBusqueda = "" & _
                    "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                    "FORMAT(IF(mp.dinputfinalprice IS NULL, IF(cim2.dinputfinalprice IS NULL, IF(cim.dinputfinalprice IS NULL, IF(bp.dinputfinalprice IS NULL, IF(cib.dinputfinalprice IS NULL, 0, cib.dinputfinalprice), bp.dinputfinalprice), cim.dinputfinalprice), cim2.dinputfinalprice), mp.dinputfinalprice), 2) AS 'Precio', " & _
                    "IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS 'Categoria' " & _
                    "FROM inputs i " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT btfci.ibaseid, btfci.icardid, btfci.iinputid, SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS dinputfinalprice FROM basecardcompoundinputs btfci LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid ORDER BY bp.iupdatedate DESC, bp.supdatetime DESC) cib GROUP BY ibaseid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cib ON i.iinputid = cib.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON i.iinputid = mp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT mtfci.imodelid, mtfci.icardid, mtfci.iinputid, SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS dinputfinalprice FROM modelcardcompoundinputs mtfci LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND i.iinputid = mp.iinputid GROUP BY mtfci.imodelid, mtfci.icardid, mtfci.iinputid ORDER BY mp.iupdatedate DESC, mp.supdatetime DESC) cim GROUP BY imodelid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cim ON i.iinputid = cim.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT mtfci.imodelid, mtfci.icardid, mtfci.iinputid, SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND i.iinputid = mp.iinputid GROUP BY mtfci.imodelid, mtfci.icardid, mtfci.iinputid ORDER BY mp.iupdatedate DESC, mp.supdatetime DESC) cim2 GROUP BY imodelid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cim2 ON i.iinputid = cim2.iinputid " & _
                    "LEFT JOIN inputcategories ic ON i.iinputid = ic.iinputid " & _
                    "WHERE i.sinputdescription LIKE '%" & querystring & "%' OR i.sinputunit LIKE '%" & querystring & "%' OR ic.sinputcategory LIKE '%" & querystring & "%' " & _
                    "GROUP BY i.iinputid " & _
                    "ORDER BY i.sinputdescription "

                Else

                    queryBusqueda = "" & _
                    "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                    "FORMAT(IF(mp.dinputfinalprice IS NULL, IF(cim.dinputfinalprice IS NULL, IF(bp.dinputfinalprice IS NULL, IF(cib.dinputfinalprice IS NULL, 0, cib.dinputfinalprice), bp.dinputfinalprice), cim.dinputfinalprice), mp.dinputfinalprice), 2) AS 'Precio', " & _
                    "IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS 'Categoria' " & _
                    "FROM inputs i " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT btfci.ibaseid, btfci.icardid, btfci.iinputid, SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS dinputfinalprice FROM basecardcompoundinputs btfci LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid ORDER BY bp.iupdatedate DESC, bp.supdatetime DESC) cib GROUP BY ibaseid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cib ON i.iinputid = cib.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON i.iinputid = mp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT mtfci.imodelid, mtfci.icardid, mtfci.iinputid, SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS dinputfinalprice FROM modelcardcompoundinputs mtfci LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND i.iinputid = mp.iinputid GROUP BY mtfci.imodelid, mtfci.icardid, mtfci.iinputid ORDER BY mp.iupdatedate DESC, mp.supdatetime DESC) cim GROUP BY imodelid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cim ON i.iinputid = cim.iinputid " & _
                    "LEFT JOIN inputcategories ic ON i.iinputid = ic.iinputid " & _
                    "WHERE i.sinputdescription LIKE '%" & querystring & "%' OR i.sinputunit LIKE '%" & querystring & "%' OR ic.sinputcategory LIKE '%" & querystring & "%' " & _
                    "GROUP BY i.iinputid " & _
                    "ORDER BY i.sinputdescription "

                End If


            Else

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE 'tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "%Project" & iprojectid & "'") > 0 Then

                    queryBusqueda = "" & _
                    "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                    "FORMAT(IF(pp.dinputfinalprice IS NULL, IF(cip2.dinputfinalprice IS NULL, IF(cip.dinputfinalprice IS NULL, IF(bp.dinputfinalprice IS NULL, IF(cib.dinputfinalprice IS NULL, 0, cib.dinputfinalprice), bp.dinputfinalprice), cip.dinputfinalprice), cip2.dinputfinalprice), pp.dinputfinalprice), 2) AS 'Precio', " & _
                    "IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS 'Categoria' " & _
                    "FROM inputs i " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT btfci.ibaseid, btfci.icardid, btfci.iinputid, SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS dinputfinalprice FROM basecardcompoundinputs btfci LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid ORDER BY bp.iupdatedate DESC, bp.supdatetime DESC) cim GROUP BY ibaseid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cib ON i.iinputid = cib.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT ptfci.iprojectid, ptfci.icardid, ptfci.iinputid, SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS dinputfinalprice FROM projectcardcompoundinputs ptfci LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid GROUP BY ptfci.iprojectid, ptfci.icardid, ptfci.iinputid ORDER BY pp.iupdatedate DESC, pp.supdatetime DESC) cip GROUP BY iprojectid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cip ON i.iinputid = cip.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT ptfci.iprojectid, ptfci.icardid, ptfci.iinputid, SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid GROUP BY ptfci.iprojectid, ptfci.icardid, ptfci.iinputid ORDER BY pp.iupdatedate DESC, pp.supdatetime DESC) cip2 GROUP BY iprojectid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cip2 ON i.iinputid = cip2.iinputid " & _
                    "LEFT JOIN inputcategories ic ON i.iinputid = ic.iinputid " & _
                    "WHERE i.sinputdescription LIKE '%" & querystring & "%' OR i.sinputunit LIKE '%" & querystring & "%' OR ic.sinputcategory LIKE '%" & querystring & "%' " & _
                    "GROUP BY i.iinputid " & _
                    "ORDER BY i.sinputdescription "

                Else

                    queryBusqueda = "" & _
                    "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                    "FORMAT(IF(pp.dinputfinalprice IS NULL, IF(cip.dinputfinalprice IS NULL, IF(bp.dinputfinalprice IS NULL, IF(cib.dinputfinalprice IS NULL, 0, cib.dinputfinalprice), bp.dinputfinalprice), cip.dinputfinalprice), pp.dinputfinalprice), 2) AS 'Precio', " & _
                    "IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS 'Categoria' " & _
                    "FROM inputs i " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT btfci.ibaseid, btfci.icardid, btfci.iinputid, SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS dinputfinalprice FROM basecardcompoundinputs btfci LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid ORDER BY bp.iupdatedate DESC, bp.supdatetime DESC) cim GROUP BY ibaseid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cib ON i.iinputid = cib.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT ptfci.iprojectid, ptfci.icardid, ptfci.iinputid, SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS dinputfinalprice FROM projectcardcompoundinputs ptfci LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid GROUP BY ptfci.iprojectid, ptfci.icardid, ptfci.iinputid ORDER BY pp.iupdatedate DESC, pp.supdatetime DESC) cip GROUP BY iprojectid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cip ON i.iinputid = cip.iinputid " & _
                    "LEFT JOIN inputcategories ic ON i.iinputid = ic.iinputid " & _
                    "WHERE i.sinputdescription LIKE '%" & querystring & "%' OR i.sinputunit LIKE '%" & querystring & "%' OR ic.sinputcategory LIKE '%" & querystring & "%' " & _
                    "GROUP BY i.iinputid " & _
                    "ORDER BY i.sinputdescription "

                End If

            End If

        End If

        setDataGridView(dgvInsumos, queryBusqueda, True)

        dgvInsumos.Columns(0).Visible = False

        dgvInsumos.Columns(0).Width = 30
        dgvInsumos.Columns(1).Width = 370
        dgvInsumos.Columns(2).Width = 70
        dgvInsumos.Columns(3).Width = 70
        dgvInsumos.Columns(4).Width = 200

        If restrictCompounds = True Then
            btnCrear.Visible = True
        End If

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Insumos', 'OK')")

        dgvInsumos.Select()
        txtBuscar.Select()
        txtBuscar.Focus()
        txtBuscar.SelectionStart() = txtBuscar.Text.Length

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


    Private Sub txtBuscar_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBuscar.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtBuscar.Text.Contains(arrayCaractProhib(carp)) Then
                txtBuscar.Text = txtBuscar.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtBuscar.Select(txtBuscar.Text.Length, 0)
        End If

        txtBuscar.Text = txtBuscar.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        querystring = txtBuscar.Text.Replace(" ", "%").Replace("--", "").Replace("'", " PIES")

        If txtBuscar.Text.Contains("'") = True Then
            txtBuscar.Text = txtBuscar.Text.Replace("'", "")
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If txtBuscar.Text.Length < 3 Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim queryBusqueda As String

        If IsBase = True Then

            If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE 'tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "%Base" & iprojectid & "'") > 0 Then

                queryBusqueda = "" & _
                "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                "FORMAT(IF(bp.dinputfinalprice IS NULL, IF(cib.dinputfinalprice IS NULL, IF(cib2.dinputfinalprice IS NULL, 0, cib2.dinputfinalprice), cib.dinputfinalprice), bp.dinputfinalprice), 2) AS 'Precio', " & _
                "IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS 'Categoria' " & _
                "FROM inputs i " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT btfci.ibaseid, btfci.icardid, btfci.iinputid, SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS dinputfinalprice FROM basecardcompoundinputs btfci LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid ORDER BY bp.iupdatedate DESC, bp.supdatetime DESC) cib GROUP BY ibaseid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cib ON i.iinputid = cib.iinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT btfci.ibaseid, btfci.icardid, btfci.iinputid, SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid ORDER BY bp.iupdatedate DESC, bp.supdatetime DESC) cib2 GROUP BY ibaseid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cib2 ON i.iinputid = cib2.iinputid " & _
                "LEFT JOIN inputcategories ic ON i.iinputid = ic.iinputid " & _
                "WHERE i.sinputdescription LIKE '%" & querystring & "%' OR i.sinputunit LIKE '%" & querystring & "%' OR ic.sinputcategory LIKE '%" & querystring & "%' " & _
                "GROUP BY i.iinputid " & _
                "ORDER BY i.sinputdescription "

            Else

                queryBusqueda = "" & _
                "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                "FORMAT(IF(bp.dinputfinalprice IS NULL, IF(cib.dinputfinalprice IS NULL, 0, cib.dinputfinalprice), bp.dinputfinalprice), 2) AS 'Precio', " & _
                "IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS 'Categoria' " & _
                "FROM inputs i " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT btfci.ibaseid, btfci.icardid, btfci.iinputid, SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS dinputfinalprice FROM basecardcompoundinputs btfci LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid ORDER BY bp.iupdatedate DESC, bp.supdatetime DESC) cib GROUP BY ibaseid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cib ON i.iinputid = cib.iinputid " & _
                "LEFT JOIN inputcategories ic ON i.iinputid = ic.iinputid " & _
                "WHERE i.sinputdescription LIKE '%" & querystring & "%' OR i.sinputunit LIKE '%" & querystring & "%' OR ic.sinputcategory LIKE '%" & querystring & "%' " & _
                "GROUP BY i.iinputid " & _
                "ORDER BY i.sinputdescription "

            End If

        Else

            If IsModel = True Then

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE 'tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "%Model" & iprojectid & "'") > 0 Then

                    queryBusqueda = "" & _
                    "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                    "FORMAT(IF(mp.dinputfinalprice IS NULL, IF(cim2.dinputfinalprice IS NULL, IF(cim.dinputfinalprice IS NULL, IF(bp.dinputfinalprice IS NULL, IF(cib.dinputfinalprice IS NULL, 0, cib.dinputfinalprice), bp.dinputfinalprice), cim.dinputfinalprice), cim2.dinputfinalprice), mp.dinputfinalprice), 2) AS 'Precio', " & _
                    "IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS 'Categoria' " & _
                    "FROM inputs i " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT btfci.ibaseid, btfci.icardid, btfci.iinputid, SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS dinputfinalprice FROM basecardcompoundinputs btfci LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid ORDER BY bp.iupdatedate DESC, bp.supdatetime DESC) cib GROUP BY ibaseid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cib ON i.iinputid = cib.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON i.iinputid = mp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT mtfci.imodelid, mtfci.icardid, mtfci.iinputid, SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS dinputfinalprice FROM modelcardcompoundinputs mtfci LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND i.iinputid = mp.iinputid GROUP BY mtfci.imodelid, mtfci.icardid, mtfci.iinputid ORDER BY mp.iupdatedate DESC, mp.supdatetime DESC) cim GROUP BY imodelid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cim ON i.iinputid = cim.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT mtfci.imodelid, mtfci.icardid, mtfci.iinputid, SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND i.iinputid = mp.iinputid GROUP BY mtfci.imodelid, mtfci.icardid, mtfci.iinputid ORDER BY mp.iupdatedate DESC, mp.supdatetime DESC) cim2 GROUP BY imodelid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cim2 ON i.iinputid = cim2.iinputid " & _
                    "LEFT JOIN inputcategories ic ON i.iinputid = ic.iinputid " & _
                    "WHERE i.sinputdescription LIKE '%" & querystring & "%' OR i.sinputunit LIKE '%" & querystring & "%' OR ic.sinputcategory LIKE '%" & querystring & "%' " & _
                    "GROUP BY i.iinputid " & _
                    "ORDER BY i.sinputdescription "

                Else

                    queryBusqueda = "" & _
                    "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                    "FORMAT(IF(mp.dinputfinalprice IS NULL, IF(cim.dinputfinalprice IS NULL, IF(bp.dinputfinalprice IS NULL, IF(cib.dinputfinalprice IS NULL, 0, cib.dinputfinalprice), bp.dinputfinalprice), cim.dinputfinalprice), mp.dinputfinalprice), 2) AS 'Precio', " & _
                    "IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS 'Categoria' " & _
                    "FROM inputs i " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT btfci.ibaseid, btfci.icardid, btfci.iinputid, SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS dinputfinalprice FROM basecardcompoundinputs btfci LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid ORDER BY bp.iupdatedate DESC, bp.supdatetime DESC) cib GROUP BY ibaseid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cib ON i.iinputid = cib.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON i.iinputid = mp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT mtfci.imodelid, mtfci.icardid, mtfci.iinputid, SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS dinputfinalprice FROM modelcardcompoundinputs mtfci LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND i.iinputid = mp.iinputid GROUP BY mtfci.imodelid, mtfci.icardid, mtfci.iinputid ORDER BY mp.iupdatedate DESC, mp.supdatetime DESC) cim GROUP BY imodelid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cim ON i.iinputid = cim.iinputid " & _
                    "LEFT JOIN inputcategories ic ON i.iinputid = ic.iinputid " & _
                    "WHERE i.sinputdescription LIKE '%" & querystring & "%' OR i.sinputunit LIKE '%" & querystring & "%' OR ic.sinputcategory LIKE '%" & querystring & "%' " & _
                    "GROUP BY i.iinputid " & _
                    "ORDER BY i.sinputdescription "

                End If


            Else

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE 'tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "%Project" & iprojectid & "'") > 0 Then

                    queryBusqueda = "" & _
                    "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                    "FORMAT(IF(pp.dinputfinalprice IS NULL, IF(cip2.dinputfinalprice IS NULL, IF(cip.dinputfinalprice IS NULL, IF(bp.dinputfinalprice IS NULL, IF(cib.dinputfinalprice IS NULL, 0, cib.dinputfinalprice), bp.dinputfinalprice), cip.dinputfinalprice), cip2.dinputfinalprice), pp.dinputfinalprice), 2) AS 'Precio', " & _
                    "IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS 'Categoria' " & _
                    "FROM inputs i " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT btfci.ibaseid, btfci.icardid, btfci.iinputid, SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS dinputfinalprice FROM basecardcompoundinputs btfci LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid ORDER BY bp.iupdatedate DESC, bp.supdatetime DESC) cim GROUP BY ibaseid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cib ON i.iinputid = cib.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT ptfci.iprojectid, ptfci.icardid, ptfci.iinputid, SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS dinputfinalprice FROM projectcardcompoundinputs ptfci LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid GROUP BY ptfci.iprojectid, ptfci.icardid, ptfci.iinputid ORDER BY pp.iupdatedate DESC, pp.supdatetime DESC) cip GROUP BY iprojectid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cip ON i.iinputid = cip.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT ptfci.iprojectid, ptfci.icardid, ptfci.iinputid, SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid GROUP BY ptfci.iprojectid, ptfci.icardid, ptfci.iinputid ORDER BY pp.iupdatedate DESC, pp.supdatetime DESC) cip2 GROUP BY iprojectid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cip2 ON i.iinputid = cip2.iinputid " & _
                    "LEFT JOIN inputcategories ic ON i.iinputid = ic.iinputid " & _
                    "WHERE i.sinputdescription LIKE '%" & querystring & "%' OR i.sinputunit LIKE '%" & querystring & "%' OR ic.sinputcategory LIKE '%" & querystring & "%' " & _
                    "GROUP BY i.iinputid " & _
                    "ORDER BY i.sinputdescription "

                Else

                    queryBusqueda = "" & _
                    "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                    "FORMAT(IF(pp.dinputfinalprice IS NULL, IF(cip.dinputfinalprice IS NULL, IF(bp.dinputfinalprice IS NULL, IF(cib.dinputfinalprice IS NULL, 0, cib.dinputfinalprice), bp.dinputfinalprice), cip.dinputfinalprice), pp.dinputfinalprice), 2) AS 'Precio', " & _
                    "IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS 'Categoria' " & _
                    "FROM inputs i " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT btfci.ibaseid, btfci.icardid, btfci.iinputid, SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS dinputfinalprice FROM basecardcompoundinputs btfci LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid ORDER BY bp.iupdatedate DESC, bp.supdatetime DESC) cim GROUP BY ibaseid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cib ON i.iinputid = cib.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM (SELECT ptfci.iprojectid, ptfci.icardid, ptfci.iinputid, SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS dinputfinalprice FROM projectcardcompoundinputs ptfci LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid GROUP BY ptfci.iprojectid, ptfci.icardid, ptfci.iinputid ORDER BY pp.iupdatedate DESC, pp.supdatetime DESC) cip GROUP BY iprojectid, iinputid, icardid) fv GROUP BY dinputfinalprice ORDER BY iinputid) cip ON i.iinputid = cip.iinputid " & _
                    "LEFT JOIN inputcategories ic ON i.iinputid = ic.iinputid " & _
                    "WHERE i.sinputdescription LIKE '%" & querystring & "%' OR i.sinputunit LIKE '%" & querystring & "%' OR ic.sinputcategory LIKE '%" & querystring & "%' " & _
                    "GROUP BY i.iinputid " & _
                    "ORDER BY i.sinputdescription "

                End If

            End If

        End If

        setDataGridView(dgvInsumos, queryBusqueda, True)

        dgvInsumos.Columns(0).Visible = False

        dgvInsumos.Columns(0).Width = 30
        dgvInsumos.Columns(1).Width = 370
        dgvInsumos.Columns(2).Width = 70
        dgvInsumos.Columns(3).Width = 70
        dgvInsumos.Columns(4).Width = 200

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvInsumos_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumos.CellClick

        Try

            If dgvInsumos.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iinputid = CInt(dgvInsumos.Rows(e.RowIndex).Cells(0).Value)
            sinputdescription = dgvInsumos.Rows(e.RowIndex).Cells(1).Value
            sinputunit = dgvInsumos.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

            iinputid = 0
            sinputdescription = ""
            sinputunit = ""

        End Try

    End Sub


    Private Sub dgvInsumos_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumos.CellContentClick

        Try

            If dgvInsumos.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iinputid = CInt(dgvInsumos.Rows(e.RowIndex).Cells(0).Value)
            sinputdescription = dgvInsumos.Rows(e.RowIndex).Cells(1).Value
            sinputunit = dgvInsumos.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

            iinputid = 0
            sinputdescription = ""
            sinputunit = ""

        End Try

    End Sub


    Private Sub dgvInsumos_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvInsumos.SelectionChanged

        Try

            If dgvInsumos.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iinputid = CInt(dgvInsumos.CurrentRow.Cells(0).Value)
            sinputdescription = dgvInsumos.CurrentRow.Cells(1).Value
            sinputunit = dgvInsumos.CurrentRow.Cells(2).Value

        Catch ex As Exception

            iinputid = 0
            sinputdescription = ""
            sinputunit = ""

        End Try

    End Sub


    Private Sub dgvInsumos_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumos.CellContentDoubleClick

        If openPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvInsumos.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iinputid = CInt(dgvInsumos.Rows(e.RowIndex).Cells(0).Value)
            sinputdescription = dgvInsumos.Rows(e.RowIndex).Cells(1).Value
            sinputunit = dgvInsumos.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

            iinputid = 0
            sinputdescription = ""
            sinputunit = ""

        End Try

        If dgvInsumos.SelectedRows.Count = 1 Then

            If IsEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim conteoCompound As Integer = 0
            Dim baseid As Integer = 0

            conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iinputid)

            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            If conteoCompound > 0 Then

                'Es un material compuesto

                If restrictCompounds = True Then
                    MsgBox("Si quieres editar los RENDIMIENTOS de este Insumo Compuesto, te sugiero que vayas a Presupuestos Base," & Chr(13) & "encuentres la tarjeta que contiene este insumo y edites desde ahí el Insumo, de manera que no afectes a" & Chr(13) & "las demás tarjetas que contienen este Insumo." & Chr(13) & "Si lo que deseas es cambiar los PRECIOS de los insumos que componen este Insumo Compuesto," & Chr(13) & "busca el insumo que cambió de precio, no éste Insumo Compuesto." & Chr(13) & "Si deseas cambiar un insumo del Insumo Compuesto, te sugiero que NO lo hagas, ya que" & Chr(13) & "podrías afectar a otros proyectos/tarjetas que usen este Insumo Compuesto. Mejor crea un Nuevo Insumo" & Chr(13) & "con los nuevos insumos y rendimientos que deseas.", MsgBoxStyle.OkOnly, "Sugerencias")
                    Exit Sub
                End If

                Dim aic As New AgregarInsumoCompuesto

                aic.susername = susername
                aic.bactive = bactive
                aic.bonline = bonline
                aic.suserfullname = suserfullname
                aic.suseremail = suseremail
                aic.susersession = susersession
                aic.susermachinename = susermachinename
                aic.suserip = suserip



                If IsBase = True Then
                    aic.iprojectid = baseid
                Else
                    aic.iprojectid = iprojectid
                End If

                aic.iinputid = iinputid

                aic.IsHistoric = IsHistoric

                aic.IsBase = IsBase
                aic.IsModel = IsModel

                aic.IsEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aic.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aic.ShowDialog(Me)
                Me.Close()


            Else

                'NO Es un material compuesto

                Dim ai As New AgregarInsumo

                ai.susername = susername
                ai.bactive = bactive
                ai.bonline = bonline
                ai.suserfullname = suserfullname
                ai.suseremail = suseremail
                ai.susersession = susersession
                ai.susermachinename = susermachinename
                ai.suserip = suserip


                If IsBase = True Then
                    ai.iprojectid = baseid
                Else
                    ai.iprojectid = iprojectid
                End If

                ai.iinputid = iinputid

                ai.IsHistoric = IsHistoric

                ai.IsBase = IsBase
                ai.IsModel = IsModel

                ai.IsEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ai.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ai.ShowDialog(Me)
                Me.Close()

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvInsumos_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumos.CellDoubleClick

        If openPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvInsumos.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iinputid = CInt(dgvInsumos.Rows(e.RowIndex).Cells(0).Value)
            sinputdescription = dgvInsumos.Rows(e.RowIndex).Cells(1).Value
            sinputunit = dgvInsumos.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

            iinputid = 0
            sinputdescription = ""
            sinputunit = ""

        End Try

        If dgvInsumos.SelectedRows.Count = 1 Then

            If IsEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim conteoCompound As Integer = 0
            Dim baseid As Integer = 0

            conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iinputid)

            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            If conteoCompound > 0 Then

                'Es un material compuesto

                If restrictCompounds = True Then
                    MsgBox("Si quieres editar los RENDIMIENTOS de este Insumo Compuesto, te sugiero que vayas a Presupuestos Base," & Chr(13) & "encuentres la tarjeta que contiene este insumo y edites desde ahí el Insumo, de manera que no afectes a" & Chr(13) & "las demás tarjetas que contienen este Insumo." & Chr(13) & "Si lo que deseas es cambiar los PRECIOS de los insumos que componen este Insumo Compuesto," & Chr(13) & "busca el insumo que cambió de precio, no éste Insumo Compuesto." & Chr(13) & "Si deseas cambiar un insumo del Insumo Compuesto, te sugiero que NO lo hagas, ya que" & Chr(13) & "podrías afectar a otros proyectos/tarjetas que usen este Insumo Compuesto. Mejor crea un Nuevo Insumo" & Chr(13) & "con los nuevos insumos y rendimientos que deseas.", MsgBoxStyle.OkOnly, "Sugerencias")
                    Exit Sub
                End If

                Dim aic As New AgregarInsumoCompuesto

                aic.susername = susername
                aic.bactive = bactive
                aic.bonline = bonline
                aic.suserfullname = suserfullname
                aic.suseremail = suseremail
                aic.susersession = susersession
                aic.susermachinename = susermachinename
                aic.suserip = suserip

                If IsBase = True Then
                    aic.iprojectid = baseid
                Else
                    aic.iprojectid = iprojectid
                End If

                aic.iinputid = iinputid

                aic.IsHistoric = IsHistoric

                aic.IsBase = IsBase
                aic.IsModel = IsModel

                aic.IsEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aic.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aic.ShowDialog(Me)
                Me.Close()


            Else

                'NO Es un material compuesto

                Dim ai As New AgregarInsumo

                ai.susername = susername
                ai.bactive = bactive
                ai.bonline = bonline
                ai.suserfullname = suserfullname
                ai.suseremail = suseremail
                ai.susersession = susersession
                ai.susermachinename = susermachinename
                ai.suserip = suserip

                If IsBase = True Then
                    ai.iprojectid = baseid
                Else
                    ai.iprojectid = iprojectid
                End If

                ai.iinputid = iinputid

                ai.IsHistoric = IsHistoric

                ai.IsBase = IsBase
                ai.IsModel = IsModel

                ai.IsEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ai.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ai.ShowDialog(Me)
                Me.Close()

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnCrear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCrear.Click

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        Dim bipni As New BuscaInsumosPreguntaTipoNuevoInsumo
        bipni.susername = susername
        bipni.bactive = bactive
        bipni.bonline = bonline
        bipni.suserfullname = suserfullname

        bipni.suseremail = suseremail
        bipni.susersession = susersession
        bipni.susermachinename = susermachinename
        bipni.suserip = suserip

        bipni.restrictCompounds = True

        bipni.ShowDialog(Me)

        If bipni.DialogResult = Windows.Forms.DialogResult.OK Then

            If bipni.iselectedoption = 1 Then

                'Nuevo Insumo Normal

                Dim ai As New AgregarInsumo
                ai.susername = susername
                ai.bactive = bactive
                ai.bonline = bonline
                ai.suserfullname = suserfullname
                ai.suseremail = suseremail
                ai.susersession = susersession
                ai.susermachinename = susermachinename
                ai.suserip = suserip

                ai.iprojectid = iprojectid
                ai.icardid = icardid

                ai.IsEdit = False
                ai.IsHistoric = False
                ai.IsModel = IsModel
                ai.IsBase = IsBase

                If mayNeedBaseTablesCreation = True Then

                    ai.iprojectid = baseid

                    Dim queriesCreation(14) As String

                    queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid
                    queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " ( `ibaseid` int(11) NOT NULL AUTO_INCREMENT, `sbasefileslocation` varchar(400) CHARACTER SET latin1 NOT NULL, `dbaseindirectpercentagedefault` decimal(20,5) NOT NULL, `dbasegainpercentagedefault` decimal(20,5) NOT NULL, `dbaseIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts"
                    queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts" & " ( `ibaseid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `sbasecostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dbasecost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icostid`), KEY `baseid` (`ibaseid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards"
                    queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `cardlegacycategoryid` (`scardlegacycategoryid`), KEY `cardlegacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs"
                    queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs"
                    queriesCreation(9) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices"
                    queriesCreation(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices" & " ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `baseid` (`ibaseid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber"
                    queriesCreation(13) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    executeTransactedSQLCommand(0, queriesCreation)


                    'Insert Info from Base

                    Dim queriesInsert(7) As String

                    queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " SELECT * FROM base WHERE ibaseid = " & baseid
                    queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts SELECT * FROM baseindirectcosts WHERE ibaseid = " & baseid
                    queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards SELECT * FROM basecards WHERE ibaseid = " & baseid
                    queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs SELECT * FROM basecardinputs WHERE ibaseid = " & baseid
                    queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs SELECT * FROM basecardcompoundinputs WHERE ibaseid = " & baseid
                    queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices SELECT * FROM baseprices WHERE ibaseid = " & baseid
                    queriesInsert(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber SELECT * FROM basetimber WHERE ibaseid = " & baseid

                    executeTransactedSQLCommand(0, queriesInsert)

                    'Show Add Window

                End If

                If Me.WindowState = FormWindowState.Maximized Then
                    ai.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ai.ShowDialog(Me)

                If ai.DialogResult = Windows.Forms.DialogResult.OK Then

                    If mayNeedBaseTablesCreation = True Then

                        Dim queriesSave(29) As String

                        queriesSave(0) = "" & _
                        "DELETE " & _
                        "FROM base " & _
                        "WHERE ibaseid = " & baseid & " AND " & _
                        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " tb WHERE base.ibaseid = tb.ibaseid) "

                        queriesSave(1) = "" & _
                        "DELETE " & _
                        "FROM baseindirectcosts " & _
                        "WHERE ibaseid = " & baseid & " AND " & _
                        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts tbic WHERE baseindirectcosts.ibaseid = tbic.ibaseid AND baseindirectcosts.icostid = tbic.icostid) "

                        queriesSave(2) = "" & _
                        "DELETE " & _
                        "FROM basecards " & _
                        "WHERE ibaseid = " & baseid & " AND " & _
                        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

                        'queriesSave(3) = "" & _
                        '"DELETE " & _
                        '"FROM basecards " & _
                        '"WHERE ibaseid = " & baseid & " AND " & _
                        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

                        'queriesSave(4) = "" & _
                        '"DELETE " & _
                        '"FROM projectcards " & _
                        '"WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE bc.ibaseid = " & baseid & " AND projectcards.icardid = bc.icardid) "

                        'queriesSave(5) = "" & _
                        '"DELETE " & _
                        '"FROM modelcards " & _
                        '"WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE bc.ibaseid = " & baseid & " AND modelcards.icardid = bc.icardid) "

                        queriesSave(6) = "" & _
                        "DELETE " & _
                        "FROM basecardinputs " & _
                        "WHERE ibaseid = " & baseid & " AND " & _
                        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs tbci WHERE basecardinputs.ibaseid = tbci.ibaseid AND basecardinputs.icardid = tbci.icardid AND basecardinputs.iinputid = tbci.iinputid) "

                        'queriesSave(7) = "" & _
                        '"DELETE " & _
                        '"FROM projectcardinputs " & _
                        '"WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE bci.ibaseid = " & baseid & " AND projectcardinputs.icardid = bci.icardid AND projectcardinputs.iinputid = bci.iinputid) "

                        'queriesSave(8) = "" & _
                        '"DELETE " & _
                        '"FROM modelcardinputs " & _
                        '"WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE bci.ibaseid = " & baseid & " AND modelcardinputs.icardid = bci.icardid AND modelcardinputs.iinputid = bci.iinputid) "

                        queriesSave(9) = "" & _
                        "DELETE " & _
                        "FROM basecardcompoundinputs " & _
                        "WHERE ibaseid = " & baseid & " AND " & _
                        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs tbcci WHERE basecardcompoundinputs.ibaseid = tbcci.ibaseid AND basecardcompoundinputs.icardid = tbcci.icardid AND basecardcompoundinputs.iinputid = tbcci.iinputid AND basecardcompoundinputs.icompoundinputid = tbcci.icompoundinputid) "

                        'queriesSave(10) = "" & _
                        '"DELETE " & _
                        '"FROM projectcardcompoundinputs " & _
                        '"WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE bcci.ibaseid = " & baseid & " AND projectcardcompoundinputs.icardid = bcci.icardid AND projectcardcompoundinputs.iinputid = bcci.iinputid AND projectcardcompoundinputs.icompoundinputid = bcci.icompoundinputid) "

                        'queriesSave(11) = "" & _
                        '"DELETE " & _
                        '"FROM modelcardcompoundinputs " & _
                        '"WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE bcci.ibaseid = " & baseid & " AND modelcardcompoundinputs.icardid = bcci.icardid AND modelcardcompoundinputs.iinputid = bcci.iinputid AND modelcardcompoundinputs.icompoundinputid = bcci.icompoundinputid) "

                        queriesSave(12) = "" & _
                        "DELETE " & _
                        "FROM baseprices " & _
                        "WHERE ibaseid = " & baseid & " AND " & _
                        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) "

                        queriesSave(13) = "" & _
                        "DELETE " & _
                        "FROM basetimber " & _
                        "WHERE ibaseid = " & baseid & " AND " & _
                        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber tbt WHERE basetimber.ibaseid = tbt.ibaseid AND basetimber.iinputid = tbt.iinputid) "

                        queriesSave(14) = "" & _
                        "UPDATE base p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " tp ON tp.ibaseid = p.ibaseid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sbasefileslocation = tp.sbasefileslocation, p.dbaseindirectpercentagedefault = tp.dbaseindirectpercentagedefault, p.dbasegainpercentagedefault = tp.dbasegainpercentagedefault, p.dbaseIVApercentage = tp.dbaseIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

                        queriesSave(15) = "" & _
                        "UPDATE baseindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts tpic ON tpic.ibaseid = pic.ibaseid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.sbasecostname = tpic.sbasecostname, pic.dbasecost = tpic.dbasecost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

                        queriesSave(16) = "" & _
                        "UPDATE basecards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards tpc ON tpc.ibaseid = pc.ibaseid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

                        queriesSave(17) = "" & _
                        "UPDATE basecardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs tpci ON tpci.ibaseid = pci.ibaseid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

                        queriesSave(18) = "" & _
                        "UPDATE basecardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs tpcci ON tpcci.ibaseid = pcci.ibaseid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

                        queriesSave(19) = "" & _
                        "UPDATE baseprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices tpp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

                        queriesSave(20) = "" & _
                        "UPDATE basetimber pt JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber tpt ON tpt.ibaseid = pt.ibaseid AND tpt.iinputid = pt.iinputid SET pt.iupdatedate = tpt.iupdatedate, pt.supdatetime = tpt.supdatetime, pt.supdateusername = tpt.supdateusername, pt.dinputtimberespesor = tpt.dinputtimberespesor, pt.dinputtimberancho = tpt.dinputtimberancho, pt.dinputtimberlargo = tpt.dinputtimberlargo, pt.dinputtimberpreciopiecubico = tpt.dinputtimberpreciopiecubico WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') "

                        queriesSave(21) = "" & _
                        "INSERT INTO base " & _
                        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " tb " & _
                        "WHERE NOT EXISTS (SELECT * FROM base b WHERE b.ibaseid = tb.ibaseid AND b.ibaseid = " & baseid & ") "

                        queriesSave(22) = "" & _
                        "INSERT INTO baseindirectcosts " & _
                        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts tbic " & _
                        "WHERE NOT EXISTS (SELECT * FROM baseindirectcosts bic WHERE tbic.ibaseid = bic.ibaseid AND tbic.icostid = bic.icostid AND bic.ibaseid = " & baseid & ") "

                        queriesSave(23) = "" & _
                        "INSERT INTO basecards " & _
                        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards tbc " & _
                        "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE tbc.ibaseid = bc.ibaseid AND tbc.icardid = bc.icardid AND bc.ibaseid = " & baseid & ") "

                        queriesSave(24) = "" & _
                        "INSERT INTO basecardinputs " & _
                        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs tbci " & _
                        "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE tbci.ibaseid = bci.ibaseid AND tbci.icardid = bci.icardid AND tbci.iinputid = bci.iinputid AND bci.ibaseid = " & baseid & ") "

                        queriesSave(25) = "" & _
                        "INSERT INTO basecardcompoundinputs " & _
                        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs tbcci " & _
                        "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE tbcci.ibaseid = bcci.ibaseid AND tbcci.icardid = bcci.icardid AND tbcci.iinputid = bcci.iinputid AND tbcci.icompoundinputid = bcci.icompoundinputid AND bcci.ibaseid = " & baseid & ") "

                        queriesSave(26) = "" & _
                        "INSERT INTO baseprices " & _
                        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices tbp " & _
                        "WHERE NOT EXISTS (SELECT * FROM baseprices bp WHERE tbp.ibaseid = bp.ibaseid AND tbp.iinputid = bp.iinputid AND tbp.iupdatedate = bp.iupdatedate AND tbp.supdatetime = bp.supdatetime AND bp.ibaseid = " & baseid & ") "

                        queriesSave(27) = "" & _
                        "INSERT INTO basetimber " & _
                        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber tbt " & _
                        "WHERE NOT EXISTS (SELECT * FROM basetimber bt WHERE tbt.ibaseid = bt.ibaseid AND tbt.iinputid = bt.iinputid AND bt.ibaseid = " & baseid & ") "

                        queriesSave(28) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Materiales directamente (Insumo ID: " & ai.iinputid & "', 'OK')"

                        If executeTransactedSQLCommand(0, queriesSave) = True Then
                            MsgBox("Insumo guardado exitosamente", MsgBoxStyle.OkOnly, "")
                        Else
                            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If

                    End If

                    Me.wasCreated = ai.wasCreated
                    iinputid = ai.iinputid

                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()

                    If mayNeedBaseTablesCreation = True Then

                        'Drop ALL base tables

                        Dim queriesDelete(9) As String

                        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid
                        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts"
                        queriesDelete(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards"
                        queriesDelete(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardsAux"
                        queriesDelete(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs"
                        queriesDelete(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs"
                        queriesDelete(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices"
                        queriesDelete(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber"

                        executeTransactedSQLCommand(0, queriesDelete)

                    End If

                Else

                    Me.Visible = True

                End If

            ElseIf bipni.iselectedoption = 2 Then

                If mayNeedBaseTablesCreation = True Then

                    MsgBox("No es posible crear un Insumo Compuesto desde esta ventana. Lo siento.")
                    Exit Sub

                End If

                Dim aic As New AgregarInsumoCompuesto
                aic.susername = susername
                aic.bactive = bactive
                aic.bonline = bonline
                aic.suserfullname = suserfullname
                aic.suseremail = suseremail
                aic.susersession = susersession
                aic.susermachinename = susermachinename
                aic.suserip = suserip

                aic.iprojectid = iprojectid
                aic.icardid = icardid

                aic.IsEdit = False
                aic.IsHistoric = False
                aic.IsModel = IsModel
                aic.IsBase = IsBase

                If Me.WindowState = FormWindowState.Maximized Then
                    aic.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aic.ShowDialog(Me)

                If aic.DialogResult = Windows.Forms.DialogResult.OK Then

                    Me.wasCreated = aic.wasCreated
                    iinputid = aic.iinputid

                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()

                Else

                    Me.Visible = True

                End If

            End If

        End If

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        querystring = ""

        iinputid = 0
        sinputdescription = ""
        sinputunit = ""

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnAbrir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbrir.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If dgvInsumos.SelectedRows.Count = 1 Then

            If IsEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim conteoCompound As Integer = 0
            Dim baseid As Integer = 0

            conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iinputid)

            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            If conteoCompound > 0 Then

                'Es un material compuesto

                If restrictCompounds = True Then
                    MsgBox("Si quieres editar los RENDIMIENTOS de este Insumo Compuesto, te sugiero que vayas a Presupuestos Base," & Chr(13) & "encuentres la tarjeta que contiene este insumo y edites desde ahí el Insumo, de manera que no afectes a" & Chr(13) & "las demás tarjetas que contienen este Insumo." & Chr(13) & "Si lo que deseas es cambiar los PRECIOS de los insumos que componen este Insumo Compuesto," & Chr(13) & "busca el insumo que cambió de precio, no éste Insumo Compuesto." & Chr(13) & "Si deseas cambiar un insumo del Insumo Compuesto, te sugiero que NO lo hagas, ya que" & Chr(13) & "podrías afectar a otros proyectos/tarjetas que usen este Insumo Compuesto. Mejor crea un Nuevo Insumo" & Chr(13) & "con los nuevos insumos y rendimientos que deseas.", MsgBoxStyle.OkOnly, "Sugerencias")
                    Exit Sub
                End If

                Dim aic As New AgregarInsumoCompuesto

                aic.susername = susername
                aic.bactive = bactive
                aic.bonline = bonline
                aic.suserfullname = suserfullname
                aic.suseremail = suseremail
                aic.susersession = susersession
                aic.susermachinename = susermachinename
                aic.suserip = suserip

                If IsBase = True Then
                    aic.iprojectid = baseid
                Else
                    aic.iprojectid = iprojectid
                End If

                aic.iinputid = iinputid

                aic.IsHistoric = IsHistoric

                aic.IsBase = IsBase
                aic.IsModel = IsModel

                aic.IsEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aic.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aic.ShowDialog(Me)
                Me.Close()


            Else

                'NO Es un material compuesto

                Dim ai As New AgregarInsumo

                ai.susername = susername
                ai.bactive = bactive
                ai.bonline = bonline
                ai.suserfullname = suserfullname
                ai.suseremail = suseremail
                ai.susersession = susersession
                ai.susermachinename = susermachinename
                ai.suserip = suserip

                If IsBase = True Then
                    ai.iprojectid = baseid
                Else
                    ai.iprojectid = iprojectid
                End If

                ai.iinputid = iinputid

                ai.IsHistoric = IsHistoric

                ai.IsBase = IsBase
                ai.IsModel = IsModel

                ai.IsEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ai.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ai.ShowDialog(Me)
                Me.Close()

            End If

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click

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



            msSaveFileDialog.FileName = "Insumos " & fecha
            msSaveFileDialog.Filter = "Excel Files (*.xls) |*.xls"
            msSaveFileDialog.DefaultExt = "*.xls"

            If msSaveFileDialog.ShowDialog() = DialogResult.OK Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                resultado = ExportToExcel(msSaveFileDialog.FileName)

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If resultado = True Then
                    MsgBox("Insumos Exportados Correctamente!" & Chr(13) & "El archivo se abrirá al dar click en OK", MsgBoxStyle.OkOnly, "Exportación Completada")
                    System.Diagnostics.Process.Start(msSaveFileDialog.FileName)
                Else
                    MsgBox("No se ha podido exportar los Insumos. Intente nuevamente.", MsgBoxStyle.OkOnly, "Error al exportar los Insumos")
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Function ExportToExcel(ByVal pth As String) As Boolean

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

            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""598.5""/>")
            fs.WriteLine("   <Column ss:Width=""102""/>")
            fs.WriteLine("   <Column ss:Width=""54.75""/>")
            fs.WriteLine("   <Column ss:Width=""307.5""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""119.25""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""126.75""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""147.75""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""72""/>")
            fs.WriteLine("   <Column ss:Width=""126""/>")

            fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
            fs.WriteLine("  <Cell ss:MergeAcross=""3"" ss:StyleID=""1""><Data ss:Type=""String"">LISTA DE INSUMOS</Data></Cell>")
            fs.WriteLine("   </Row>")


            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", "Fecha:"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate()) & " " & getAppTime()))
            fs.WriteLine("    </Row>")


            'Write the grid headers columns (taken out since columns are already defined)
            'For Each col As DataGridViewColumn In dgv.Columns
            '    If col.Visible Then
            '        fs.WriteLine(String.Format("    <Column ss:Width=""{0}""></Column>", col.Width))
            '    End If
            'Next

            'Write the grid headers
            fs.WriteLine("    <Row ss:AutoFitHeight=""0"" ss:Height=""22.5"">")

            For Each col As DataGridViewColumn In dgvInsumos.Columns
                If col.Visible Then
                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""2""><Data ss:Type=""String"">{0}</Data></Cell>", col.HeaderText))
                End If
            Next

            fs.WriteLine("    </Row>")

            ' Write contents for each cell
            For Each row As DataGridViewRow In dgvInsumos.Rows

                If dgvInsumos.AllowUserToAddRows = True And row.Index = dgvInsumos.Rows.Count - 1 Then
                    Exit For
                End If

                fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))

                For Each col As DataGridViewColumn In dgvInsumos.Columns

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

            'Write the separation between results and totals
            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")

            'Write the totals 
            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")


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


End Class