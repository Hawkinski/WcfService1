using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class HRMSConstant
    {
        //Menu ID
        public const string modelStateErrorMessage = "Something goes wrong! Please try Again!";
        public const string moduleName = "HRMS";

        public const string currency = "AED";

        #region -- Menu Group Constants --
        public const string leaveGroup = "HRMM25";
        public const string loanGroup = "HRMM20";
        public const string leaveApplicationGroup = "HRMM11";
        public const string leaveSettlementGroup = "HRMM07";
        public const string payrollGenerationGroup = "HRMM01";
        public const string payrollAdjustmentGroup = "HRMM06";
        public const string payrollDistributionGroup = "HRMM23";
        public const string benefitManagementGroup = "HRMM02";
        public const string finalSettlementGroup = "HRMM05";


        public const string gratuityTT = "HRMT13";
        public const string leaveTT = "HRMT12";
        public const string bonusTT = "HRMG15";
        public const string airfareTT = "HRMT17";


        public const string gratuityDEDTT = "HRMT30";
        public const string leaveDEDTT = "HRMT31";
        public const string airfareDEDTT = "HRMT01";


        public const string gratuityADJTT = "HRMT10";
        public const string leaveADJTT = "HRMT11";
        public const string airfareADJTT = "HRMT01";


        #endregion

        public const string payrollDEMC = "PDE";
        public const string payrollPostedMC = "PayPOSTED";
        public const string payrollAdjustmentDEMC = "PayAdjDE";
        public const string payrollAdjustmentPostedDEMC = "PayAdjPosDE";
        public const string payrollGenerationMC = "PG";
        public const string payrollChecklistMC = "PAYROLLCL";
        public const string payrollAdjChecklistMC = "PAYROLLADJCL";
        public const string payrollDistChecklistMC = "PAYROLLDISTCL";
        public const string leavePlannerMC = "LveP";
        public const string leavePlannerChecklistMC = "LvePCL";
        public const string loanDataEntryMC = "LoanDE";
        public const string loanPostedTransactionMC = "LoanPT";
        public const string loanChecklistMC = "LoanCL";
        public const string payrollDistributionDEMC = "WPSDE";
        
        public const string payrollDistributionPostedMC = "WPSPOSTEDDE";

        public const string payrollDeletionMC = "PAYDEL";
        public const string payrollPostingMC = "PAYPOST";
        public const string loanPostingMC = "LOANPOST";
        public const string payrollAdjustmentPostingMC = "PAYADJPOST";
        public const string finalSettlementDEMC = "FSDE";        
        public const string finalSettlementPostingMC = "FSPOST";
        public const string leaveApplicationPostedMC = "LVEAPPPOSTED";
        public const string leaveApplicationDEMC = "LVEDE";
        public const string leaveApplicationPostingMC = "LVEAPPPOST";
        public const string leaveApplicationChecklistMC = "LVECL";
        public const string leaveApplicationRevMC = "LVEAPPREV";    

        public const string benefitGenerationMC = "BG";
        public const string benefitDEMC = "BDE";
        public const string benefitPostingMC = "BPOST";
        public const string benefitPostedMC = "BPOSTED";
        public const string benefitChecklistMC = "BCL";
        public const string finalSettlementPostedMC = "FSPOSTED";
        public const string finalSettlementChecklistMC = "FSCL";
        public const string benefitDeletionMC = "BENDEL";
        public const string leaveSettlementDEMC = "LVESET";
        public const string leaveSettlementPostedMC = "LVESETPOSTED";
        public const string leaveSettlementChecklistMC = "LVESETCL";

        public const string leaveSettlementPostingMC = "LVESETPOST";

        #region -- Report Type Constants --
        public const string employeeSummaryReport = "RPT_ESR";
        public const string documentExpiryReport = "RPT_DOC_EXPIRY";
        public const string documentExpiryReportSummary = "RPT_DOC_EXPIRY_SUMRY";
        public const string documentOverDueList = "RPT_DOC_OVERDUE";
        public const string entitlementExpiryReport = "RPT_ENT_EXP";
        public const string exitCheckListReport = "RPT_Ext_checkList";
        public const string loanReport = "RPT_Loan";
        public const string leaveStatus = "RPT_LV_ST";
        public const string leaveDetail = "RPT_LV_DT";
        public const string paySlip = "RPT_PY_SLP";
        public const string coinageAnalysis = "RPT_CN_ALS";
        public const string payrollSummary = "RPT_PY_SMRY";
        public const string bankTransfer = "RPT_BK_TFR";
        public const string modeOfPayment = "RPT_MOP";
        public const string multiLevelReport = "RPT_MLR";
        public const string deptEmpTTReport = "RPT_DE_TT";
        public const string multilevelVariance = "RPT_ML_VER";
        public const string multilevelPayrollHistory = "RPT_ML_PYH";
        public const string empWiseMonthlySalDetailReport = "RPT_EW_MSD";
        public const string empWiseMonthlySalDetailHRReport = "RPT_EW_MSD_HR";
        public const string divWiseAndContWisePayrollDetailReport = "RPT_DW_MS_DT";
        public const string divWiseAndContWisePayrollSummaryReport = "RPT_DV_MS_SM";
        public const string statementOfAccount = "RPT_SOA";
        public const string staffLedger = "RPT_SL";
        public const string staffDetail = "RPT_SD";
        public const string gratuityAccrual = "RPT_GR_ACCR";
        public const string airFareAccrual = "RPT_AF_ACCR";
        public const string leaveAccrual = "RPT_LV_ACCR";
        public const string airTicketDue = "RPT_AR_DUE";
        public const string empMasterRptDesigner = "RPT_EMP_MR_RPT_DNR";
        #endregion

        #region -- Popup Dialog Width Constants --
        public const string reportDialogMaxWidth = "650";
        public const string reportDialogXLWidth = "850";
        public const string reportDialogMinWidth = "450";
        public const string dalogNormalWidth = "550";

        #endregion
    }

    public class CommonMessage
    {
        //Method
        // 101 - Insert
        // 102 - Update
        // 103 - Delete
        // 104 - Dependency
        // 105 - Duplicate
        // 500 - Internal Server Error
        // 107 - Nothing to save
        // 108 - Invalid model state
        // 109 - Posted successfully!
        // 110 - document no. duplicate 
        // 111 - Can't delete or edit posted data.
        // 112 - Employee exist.
        // 113 - Nothing to post.
        public string GetMessage(int method)
        {
            string responseMessage = "";
            switch (method)
            {
                case 101:
                    responseMessage = "Saved successfully!";
                    break;
                case 102:
                    responseMessage = "Updated successfully!";
                    break;
                case 103:
                    responseMessage = "Deleted successfully!";
                    break;
                case 104:
                    responseMessage = "Dependency Error";
                    break;
                case 105:
                    responseMessage = "Duplicate Entry";
                    break;
                case 500:
                    responseMessage = "Internal Server Error";
                    break;
                case 107:
                    responseMessage = "Nothing to save";
                    break;
                case 108:
                    responseMessage = "Invalid model state";
                    break;
                case 109:
                    responseMessage = "Posted Successfully!";
                    break;
                case 110:
                    responseMessage = "Document number exist.";
                    break;
                case 111:
                    responseMessage = "Can't delete or edit posted data";
                    break;
                case 112:
                    responseMessage = "An employee with the same code already exists.Please try with other code.";
                    break;
                case 113:
                    responseMessage = "Nothing to post.";
                    break;
            }
            return responseMessage;           
        }
    }
}
