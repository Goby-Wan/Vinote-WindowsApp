Imports System.Net
Imports System.Runtime.Serialization.Json
Imports System.IO
Imports System.Text


Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs)



    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.ColumnCount = 4

        DataGridView1.Columns(0).Name = "id"
        DataGridView1.Columns(1).Name = "Domaine"
        DataGridView1.Columns(2).Name = "Nom"
        DataGridView1.Columns(3).Name = "Prénom"

        dgridRefresh()

    End Sub

    Private Sub dgridRefresh()
        Dim url As String
        url = "http://10.0.75.1/app_dev.php/producteurs"

        Dim synClient As WebClient
        synClient = New WebClient()

        Dim content As String
        content = synClient.DownloadString(url)

        Dim serializer As DataContractJsonSerializer
        serializer = New DataContractJsonSerializer(GetType(Producteur()))

        Dim ms As New MemoryStream(Encoding.Unicode.GetBytes(content))

        Dim obj As Producteur()
        obj = serializer.ReadObject(ms)

        DataGridView1.Rows.Clear()

        For Each prod As Producteur In obj
            Dim dg As String()
            dg = prod.toArrayString()
            DataGridView1.Rows.Add(dg)
        Next
    End Sub

    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        Dim content As String
        content = "{""domaine"":""" + DataGridView1.Rows(e.RowIndex).Cells(1).Value +
            """,""nom"":""" + DataGridView1.Rows(e.RowIndex).Cells(2).Value + """,""prenom"":""" + DataGridView1.Rows(e.RowIndex).Cells(3).Value + """}"
        Dim data = Encoding.UTF8.GetBytes(content)
        Dim uri As New Uri("http://10.0.75.1/app_dev.php/producteurs/" + DataGridView1.Rows(e.RowIndex).Cells(0).Value)
        SendRequest(uri, data, "application/json", "PUT")
    End Sub

    Private Function SendRequest(uri As Uri, jsonDataBytes As Byte(), contentType As String, method As String) As String
        Dim req As WebRequest = WebRequest.Create(uri)
        req.ContentType = contentType
        req.Method = method
        req.ContentLength = jsonDataBytes.Length


        Dim stream = req.GetRequestStream()
        stream.Write(jsonDataBytes, 0, jsonDataBytes.Length)
        stream.Close()

        Dim response = req.GetResponse().GetResponseStream()

        Dim reader As New StreamReader(response)
        Dim res = reader.ReadToEnd()
        reader.Close()
        response.Close()

        Return res
    End Function

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim content As String
        content = "{""domaine"":""" + domaine.Text +
            """,""nom"":""" + nom.Text + """,""prenom"":""" + prenom.Text + """}"
        Dim data = Encoding.UTF8.GetBytes(content)
        Dim uri As New Uri("http://10.0.75.1/app_dev.php/producteurs")
        SendRequest(uri, data, "application/json", "POST")
        dgridRefresh()
        domaine.Text = ""
        nom.Text = ""
        prenom.Text = ""
    End Sub
End Class
