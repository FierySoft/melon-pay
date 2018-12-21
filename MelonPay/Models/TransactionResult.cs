using System;

namespace MelonPay.Models
{
    public class TransactionResult
    {
        public bool Success { get; set; }
        public string ErrorReason { get; set; }


        public static TransactionResult FromSuccess()
        {
            return new TransactionResult { Success = true };
        }

        public static TransactionResult FromError(string error)
        {
            return new TransactionResult { Success = false, ErrorReason = error };
        }

        public static TransactionResult FromException(Exception ex)
        {
            return TransactionResult.FromError(ex.Message);
        }
    }
}
