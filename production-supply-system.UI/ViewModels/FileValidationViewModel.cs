using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL.Models.Docmapper;
using DAL.Models.Master;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using NavigationManager.Frame.Extension.WPF;

namespace UI_Interface.ViewModels
{
    public class FileValidationViewModel : ObservableObject, INavigationAware
    {
        public FileValidationViewModel()
        {

        }

        public DataTable FileDataTable { get; set; }

        public List<Type> ValidatedModels { get; set; } = new();

        public void OnNavigatedFrom()
        {
            //ValidatedModels.Add(typeof());
        }

        public void OnNavigatedTo(object parameter)
        {
            if (parameter is ProcessStep processStep)
            {
                FileDataTable = FillDataTable(processStep);
            }
        }

        private string TrimSpecialCharacters(string originalString)
        {
            char[] charsToTrim = { ':', '.' };

            return originalString.TrimEnd(charsToTrim);
        }

        private DataTable FillDataTable(ProcessStep processStep)
        {
            DataTable fileDataTable = new();

            int firstRow = processStep.Document.FirstDataRow - 1;

            for (int i = 0; i < processStep.Document.Data.GetLength(1); i++)
            {
                object value = processStep.Document.Data[firstRow, i];

                _ = fileDataTable.Columns.Add(value is null ? null : TrimSpecialCharacters(value.ToString()));
            }

            for (int i = 0; i < processStep.Document.Data.GetLength(0); i++)
            {
                DataRow row = fileDataTable.NewRow();

                for (int j = 0; j < processStep.Document.Data.GetLength(1); j++)
                {
                    object value = processStep.Document.Data[i, j];

                    row[j] = value;
                }

                fileDataTable.Rows.Add(row);
            }

            //_ = processStep.Document.Content.OrderBy(contentItem => contentItem.DocumentColumn);

            bool isSelfCellsValid = ValidateSelfCells(processStep);

            bool isTableCellsValid = ValidateTableCells(processStep);

            return fileDataTable;
        }

        private bool ValidateTableCells(ProcessStep processStep)
        {
            for (int i = 0; i < processStep.Document.Data.GetLength(0); i++)
            {
                foreach (DocumentContent contentItem in processStep.Document.Content)
                {
                    if (contentItem.RowNumber is null)
                    {
                        if (i >= processStep.Document.FirstDataRow)
                        {
                            string systemColName = contentItem.DocumentColumn.SystemColumnName;

                            int column = contentItem.ColumnNumber - 1;

                            object value = processStep.Document.Data[i, column];

                            // Далее можно выполнять логику с текущим значением
                        }
                    }
                }
            }

            return true;
        }

        private bool ValidateSelfCells(ProcessStep processStep)
        {
            foreach (DocumentContent item in processStep.Document.Content)
            {
                if (item.RowNumber is not null)
                {
                    object val = processStep.Document.Data[(int)item.RowNumber - 1, item.ColumnNumber - 1];
                }
            }

            return true;
        }
    }
}
