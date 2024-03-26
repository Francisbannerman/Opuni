using OfficeOpenXml;
using Referral.Repositories;

namespace Referral.Services;

public class ReportService
{
    private readonly IUnitOfWork _unitOfWork;
    public ReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public byte[] ExcelReport()
    {
        var clients = _unitOfWork.Client.GetAll();

        using (var package = new ExcelPackage())
        {
            var clientsWorkSheet = package.Workbook.Worksheets.Add("Clientelle");
            clientsWorkSheet.Cells[1, 1].Value = "Client_Id";
            clientsWorkSheet.Cells[1, 2].Value = "First_Name";
            clientsWorkSheet.Cells[1, 3].Value = "Last_Name";
            clientsWorkSheet.Cells[1, 4].Value = "Tel_Number";
            clientsWorkSheet.Cells[1, 5].Value = "Referral_Code";
            clientsWorkSheet.Cells[1, 6].Value = "Date_Created";
            clientsWorkSheet.Cells[1, 7].Value = "Created_Using_Which_Referral_Number";
            clientsWorkSheet.Cells[1, 8].Value = "NumberOfTime_This_Referral_Code_Has_Been_Used";

            var itemsRow = 2;
            foreach (var client in clients)
            {
                clientsWorkSheet.Cells[itemsRow, 1].Value = client.Id;
                clientsWorkSheet.Cells[itemsRow, 2].Value = client.FirstName;
                clientsWorkSheet.Cells[itemsRow, 3].Value = client.LastName;
                clientsWorkSheet.Cells[itemsRow, 4].Value = client.TelephoneNumber;
                clientsWorkSheet.Cells[itemsRow, 5].Value = client.ReferralCode;
                clientsWorkSheet.Cells[itemsRow, 6].Value = client.DateCreated;
                clientsWorkSheet.Cells[itemsRow, 7].Value = client.CreatedUsingReferralCode;
                clientsWorkSheet.Cells[itemsRow, 8].Value = client.NumberOfTimeReferralHasBeenUsed;
                itemsRow++;
            }
            byte[] excelBytes = package.GetAsByteArray();
            return excelBytes;
        }
    }
}