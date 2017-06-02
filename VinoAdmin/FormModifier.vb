Imports System.Text

Public Class FormModifier

    Dim dicoData As Dictionary(Of String, String)
    Dim typeObjet As String
    Dim fenetreP As Form1

    Public Sub init(dic As Dictionary(Of String, String), typeOb As String, fenP As Form1)
        dicoData = dic
        typeObjet = typeOb
        fenetreP = fenP
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub FormModifier_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim x As Integer
        Dim y As Integer
        Dim x_tab As Integer
        Dim y_tab As Integer

        x = 50
        y = 30
        x_tab = 200
        y_tab = 50

        For Each kvp As KeyValuePair(Of String, String) In dicoData
            Dim label = New Label
            label.Text = kvp.Key
            label.Size = New System.Drawing.Size(x_tab - 50, y_tab - 10)
            label.Location = New System.Drawing.Point(x, y)
            Me.Controls.Add(label)

            Dim input = New TextBox
            input.Text = kvp.Value
            input.Name = kvp.Key
            input.Size = New System.Drawing.Size(x_tab - 50, y_tab - 10)
            input.Location = New System.Drawing.Point(x + x_tab, y)
            Me.Controls.Add(input)

            y = y + y_tab

        Next
        Me.Size = New System.Drawing.Size(500, y + 100)
        Button1.Location = New Point(100, y)
        Button2.Location = New Point(300, y)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Créer une chaine json
        Dim content As String
        content = "{"
        Dim premier As Boolean
        premier = True
        For Each kvp As KeyValuePair(Of String, String) In dicoData
            If (Not premier) Then
                content = content + ","
            Else
                premier = False
            End If
            Select Case kvp.Key
                Case "id", "annee", "alcool", "prix", "score", "cp", "timestamp", "note"
                    content = content + """" + kvp.Key + """:" + Me.Controls.Item(kvp.Key).Text + ""
                Case Else
                    content = content + """" + kvp.Key + """:""" + Me.Controls.Item(kvp.Key).Text + """"
            End Select
        Next
        content = content + "}"
        Console.WriteLine(content)

        Dim uri As New Uri("http://192.168.2.130/vinote/web/app_dev.php/" & typeObjet & "/" & Me.Controls.Item("id").Text)
        Dim data = Encoding.UTF8.GetBytes(content)
        Console.WriteLine(uri)
        Console.WriteLine(content)

        Form1.SendRequest(uri, data, "application/json", "PUT")
        'Form1.btCharge_Click(sender, e)

        MsgBox("Modification effectuée")
        fenetreP.chargement()
        fenetreP.changementListe()
        Me.Close()

    End Sub
End Class