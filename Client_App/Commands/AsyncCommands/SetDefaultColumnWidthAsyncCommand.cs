using Avalonia.Controls;
using Client_App.ViewModels.Forms;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

public class SetDefaultColumnWidthAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not DataGrid dataGrid || dataGrid.DataContext is null) return;

        var columns = dataGrid.Columns;

        var formNum = ((BaseFormVM)dataGrid.DataContext).FormType;

        switch (formNum)
        {
            case "1.1":
            {
                columns[0].Width = new DataGridLength(30);
                columns[1].Width = new DataGridLength(59);
                columns[2].Width = new DataGridLength(92);
                columns[3].Width = new DataGridLength(80);
                columns[4].Width = new DataGridLength(76);
                columns[5].Width = new DataGridLength(115);
                columns[6].Width = new DataGridLength(52);
                columns[7].Width = new DataGridLength(70);
                columns[8].Width = new DataGridLength(68);
                columns[9].Width = new DataGridLength(77);
                columns[10].Width = new DataGridLength(91);
                columns[11].Width = new DataGridLength(58);
                columns[12].Width = new DataGridLength(55);
                columns[13].Width = new DataGridLength(84);
                columns[14].Width = new DataGridLength(100);
                columns[15].Width = new DataGridLength(45);
                columns[16].Width = new DataGridLength(60);
                columns[17].Width = new DataGridLength(92);
                columns[18].Width = new DataGridLength(77);
                columns[19].Width = new DataGridLength(77);
                columns[20].Width = new DataGridLength(88);
                columns[21].Width = new DataGridLength(70);
                columns[22].Width = new DataGridLength(60);
                break;
            }
            case "1.2":
            {
                columns[0].Width = new DataGridLength(30);
                columns[1].Width = new DataGridLength(59);
                columns[2].Width = new DataGridLength(92);
                columns[3].Width = new DataGridLength(101);
                columns[4].Width = new DataGridLength(132);
                columns[5].Width = new DataGridLength(87);
                columns[6].Width = new DataGridLength(79);
                columns[7].Width = new DataGridLength(79);
                columns[8].Width = new DataGridLength(93);
                columns[9].Width = new DataGridLength(45);
                columns[10].Width = new DataGridLength(87);
                columns[11].Width = new DataGridLength(102);
                columns[12].Width = new DataGridLength(44);
                columns[13].Width = new DataGridLength(92);
                columns[14].Width = new DataGridLength(95);
                columns[15].Width = new DataGridLength(93);
                columns[16].Width = new DataGridLength(90);
                columns[17].Width = new DataGridLength(87);
                columns[18].Width = new DataGridLength(89);
                columns[19].Width = new DataGridLength(99);

                break;
            }
            case "1.3":
            {
                columns[0].Width = new DataGridLength(30);
                columns[1].Width = new DataGridLength(59);
                columns[2].Width = new DataGridLength(92);
                columns[3].Width = new DataGridLength(83);
                columns[4].Width = new DataGridLength(95);
                columns[5].Width = new DataGridLength(117);
                columns[6].Width = new DataGridLength(64);
                columns[7].Width = new DataGridLength(87);
                columns[8].Width = new DataGridLength(79);
                columns[9].Width = new DataGridLength(93);
                columns[10].Width = new DataGridLength(66);
                columns[11].Width = new DataGridLength(88);
                columns[12].Width = new DataGridLength(103);
                columns[13].Width = new DataGridLength(52);
                columns[14].Width = new DataGridLength(76);
                columns[15].Width = new DataGridLength(95);
                columns[16].Width = new DataGridLength(93);
                columns[17].Width = new DataGridLength(90);
                columns[18].Width = new DataGridLength(84);
                columns[19].Width = new DataGridLength(81);
                columns[20].Width = new DataGridLength(47);

                break;
            }
            case "1.4":
            {
                columns[0].Width = new DataGridLength(30);
                columns[1].Width = new DataGridLength(59);
                columns[2].Width = new DataGridLength(92);
                columns[3].Width = new DataGridLength(96);
                columns[4].Width = new DataGridLength(88);
                columns[5].Width = new DataGridLength(48);
                columns[6].Width = new DataGridLength(123);
                columns[7].Width = new DataGridLength(72);
                columns[8].Width = new DataGridLength(91);
                columns[9].Width = new DataGridLength(67);
                columns[10].Width = new DataGridLength(67);
                columns[11].Width = new DataGridLength(67);
                columns[12].Width = new DataGridLength(86);
                columns[13].Width = new DataGridLength(102);
                columns[14].Width = new DataGridLength(49);
                columns[15].Width = new DataGridLength(73);
                columns[16].Width = new DataGridLength(92);
                columns[17].Width = new DataGridLength(75);
                columns[18].Width = new DataGridLength(75);
                columns[19].Width = new DataGridLength(86);
                columns[20].Width = new DataGridLength(76);
                columns[21].Width = new DataGridLength(60);

                break;
            }
            case "1.5":
            {
                columns[0].Width = new DataGridLength(30);
                columns[1].Width = new DataGridLength(59);
                columns[2].Width = new DataGridLength(92);
                columns[3].Width = new DataGridLength(111);
                columns[4].Width = new DataGridLength(70);
                columns[5].Width = new DataGridLength(137);
                columns[6].Width = new DataGridLength(61);
                columns[7].Width = new DataGridLength(70);
                columns[8].Width = new DataGridLength(67);
                columns[9].Width = new DataGridLength(92);
                columns[10].Width = new DataGridLength(65);
                columns[11].Width = new DataGridLength(50);
                columns[12].Width = new DataGridLength(79);
                columns[13].Width = new DataGridLength(92);
                columns[14].Width = new DataGridLength(74);
                columns[15].Width = new DataGridLength(77);
                columns[16].Width = new DataGridLength(87);
                columns[17].Width = new DataGridLength(70);
                columns[18].Width = new DataGridLength(64);
                columns[19].Width = new DataGridLength(86);
                columns[20].Width = new DataGridLength(48);
                columns[21].Width = new DataGridLength(84);
                columns[22].Width = new DataGridLength(62);
                columns[23].Width = new DataGridLength(81);
                columns[24].Width = new DataGridLength(53);

                break;
            }
            case "1.6":
            {
                columns[0].Width = new DataGridLength(30);
                columns[1].Width = new DataGridLength(59);
                columns[2].Width = new DataGridLength(92);
                columns[3].Width = new DataGridLength(81);
                columns[4].Width = new DataGridLength(67);
                columns[5].Width = new DataGridLength(66);
                columns[6].Width = new DataGridLength(64);
                columns[7].Width = new DataGridLength(70);
                columns[8].Width = new DataGridLength(139);
                columns[9].Width = new DataGridLength(81);
                columns[10].Width = new DataGridLength(104);
                columns[11].Width = new DataGridLength(114);
                columns[12].Width = new DataGridLength(90);
                columns[13].Width = new DataGridLength(92);
                columns[14].Width = new DataGridLength(48);
                columns[15].Width = new DataGridLength(65);
                columns[16].Width = new DataGridLength(92);
                columns[17].Width = new DataGridLength(92);
                columns[18].Width = new DataGridLength(76);
                columns[19].Width = new DataGridLength(88);
                columns[20].Width = new DataGridLength(64);
                columns[21].Width = new DataGridLength(88);
                columns[22].Width = new DataGridLength(87);
                columns[23].Width = new DataGridLength(80);
                columns[24].Width = new DataGridLength(70);
                columns[25].Width = new DataGridLength(62);
                columns[26].Width = new DataGridLength(82);
                columns[27].Width = new DataGridLength(57);

                break;
            }
            case "1.7":
            {
                columns[0].Width = new DataGridLength(30);
                columns[1].Width = new DataGridLength(59);
                columns[2].Width = new DataGridLength(92);
                columns[3].Width = new DataGridLength(89);
                columns[4].Width = new DataGridLength(72);
                columns[5].Width = new DataGridLength(67);
                columns[6].Width = new DataGridLength(121);
                columns[7].Width = new DataGridLength(92);
                columns[8].Width = new DataGridLength(60);
                columns[9].Width = new DataGridLength(75);
                columns[10].Width = new DataGridLength(75);
                columns[11].Width = new DataGridLength(130);
                columns[12].Width = new DataGridLength(97);
                columns[13].Width = new DataGridLength(46);
                columns[14].Width = new DataGridLength(84);
                columns[15].Width = new DataGridLength(92);
                columns[16].Width = new DataGridLength(71);
                columns[17].Width = new DataGridLength(76);
                columns[18].Width = new DataGridLength(86);
                columns[19].Width = new DataGridLength(70);
                columns[20].Width = new DataGridLength(80);
                columns[21].Width = new DataGridLength(50);
                columns[22].Width = new DataGridLength(92);
                columns[23].Width = new DataGridLength(74);
                columns[24].Width = new DataGridLength(72);
                columns[25].Width = new DataGridLength(89);
                columns[26].Width = new DataGridLength(142);
                columns[27].Width = new DataGridLength(126);
                columns[28].Width = new DataGridLength(120);
                columns[29].Width = new DataGridLength(106);
                columns[30].Width = new DataGridLength(64);
                columns[31].Width = new DataGridLength(80);
                columns[32].Width = new DataGridLength(62);

                break;
            }
            case "1.8":
            {
                columns[0].Width = new DataGridLength(30);
                columns[1].Width = new DataGridLength(59);
                columns[2].Width = new DataGridLength(92);
                columns[3].Width = new DataGridLength(125);
                columns[4].Width = new DataGridLength(98);
                columns[5].Width = new DataGridLength(85);
                columns[6].Width = new DataGridLength(85);
                columns[7].Width = new DataGridLength(103);
                columns[8].Width = new DataGridLength(136);
                columns[9].Width = new DataGridLength(85);
                columns[10].Width = new DataGridLength(55);
                columns[11].Width = new DataGridLength(87);
                columns[12].Width = new DataGridLength(92);
                columns[13].Width = new DataGridLength(91);
                columns[14].Width = new DataGridLength(87);
                columns[15].Width = new DataGridLength(87);
                columns[16].Width = new DataGridLength(87);
                columns[17].Width = new DataGridLength(53);
                columns[18].Width = new DataGridLength(66);
                columns[19].Width = new DataGridLength(85);
                columns[20].Width = new DataGridLength(85);
                columns[21].Width = new DataGridLength(104);
                columns[22].Width = new DataGridLength(110);
                columns[23].Width = new DataGridLength(117);
                columns[24].Width = new DataGridLength(91);
                columns[25].Width = new DataGridLength(106);
                columns[26].Width = new DataGridLength(63);
                columns[27].Width = new DataGridLength(79);
                columns[28].Width = new DataGridLength(55);

                break;
            }
            case "1.9":
            {
                columns[0].Width = new DataGridLength(30);
                columns[1].Width = new DataGridLength(71);
                columns[2].Width = new DataGridLength(98);
                columns[3].Width = new DataGridLength(158);
                columns[4].Width = new DataGridLength(172);
                columns[5].Width = new DataGridLength(95);
                columns[6].Width = new DataGridLength(125);
                columns[7].Width = new DataGridLength(217);
                columns[8].Width = new DataGridLength(171);

                break;
            }
            default:
            {
                columns[0].Width = new DataGridLength(40);
                for (var i = 1; i < columns.Count; i++)
                {
                    columns[i].Width = dataGrid.ColumnWidth; //default is 125
                }

                break;
            }
        }
    }
}