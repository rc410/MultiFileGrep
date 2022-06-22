using System.Data;
using System.Text;

namespace MultiFileGrep
{
    public partial class Form1 : Form
    {
        private DataTable iSearchTable = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataColumn column;
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "SearchValue";
            column.AutoIncrement = false;
            column.Caption = "SearchValue";
            column.ReadOnly = false;
            column.Unique = false;

            iSearchTable.Columns.Add(column);

            dataGridView1.DataSource = iSearchTable;
            dataGridView1.Columns[0].Width = dataGridView1.Width - dataGridView1.RowHeadersWidth - 15;
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>検索実行</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            dataGridView1.EndEdit();

            iSearchTable = (DataTable)dataGridView1.DataSource;
            foreach (DataRow dr in iSearchTable.Rows)
            {
                string sarchWord = dr["SearchValue"].ToString();
                List<string> rl = FileGrep(PathTextBox.Text, sarchWord);
                SetResultList(sarchWord, rl);
            }
        }
        private List<string> FileGrep(string pTargetPath, string pSearchWord)
        {
            List<string> retList = new List<string>();
            foreach (string f in Directory.GetFiles(pTargetPath
                                                , "*.*"
                                                , SearchOption.AllDirectories))
            {
                var grep = File.ReadAllLines(@f)
                    .Select((s, i) => new { Index = i, Value = s })
                    .Where(s => s.Value.Contains(pSearchWord));
                if (grep.Count() > 0)
                {
                    retList.Add(f);
                }
            }
            return retList;
        }
        private void SetResultList(string sarchWord, List<string> rl)
        {
            if (listView1.Columns.Count == 0)
            {
                listView1.Columns.Add("File Name", 200, HorizontalAlignment.Left);
            }
            //カラム追加
            int colIdx = listView1.Columns.Count;
            { listView1.Columns.Add(sarchWord, 100, HorizontalAlignment.Left); }

            //リスト処理
            foreach (string fn in rl)
            {
                bool foundFlg = false;
                for (int pos = 0; pos < listView1.Items.Count; pos++)
                {
                    if (listView1.Items[pos].Text == fn)
                    {
                        foundFlg = true;
                        listView1.Items[pos].SubItems.Add("○");
                        break;
                    }
                }
                if (!foundFlg)
                {
                    ListViewItem item0 = listView1.Items.Add(fn);
                    for (int addIdx = 0; addIdx < colIdx; addIdx++)
                    {
                        item0.SubItems.Add("");
                    }
                    item0.SubItems[colIdx].Text = "○";
                }
            }
        }

    }
}