import React, { useState } from 'react';

type PaymentFormProps = {
  companyName: string;
  personName: string; // If you want to display the logged-in user or a customer
  category: string;
  account: string;
  onPay: (amount: number) => void;
  onCancel: () => void;
};

const PaymentForm: React.FC<PaymentFormProps> = ({
  companyName,
  personName,
  category,
  account,
  onPay,
  onCancel,
}) => {
  const [amount, setAmount] = useState('');

  // Simple validation: must be positive number
  const isValid = !!amount && !isNaN(Number(amount)) && Number(amount) > 0;

  return (
    <div className="bg-secondary rounded-lg shadow-md p-6 w-full max-w-md mx-auto mt-8">
      <h2 className="text-xl font-bold mb-6 text-primary text-center">Pay Bill</h2>
      <div className="mb-4">
        <div className="flex flex-col gap-1 mb-2">
          <span className="font-semibold text-primary">Company:</span>
          <span>{companyName}</span>
        </div>
        <div className="flex flex-col gap-1 mb-2">
          <span className="font-semibold text-primary">Account Holder:</span>
          <span>{personName}</span>
        </div>
        <div className="flex flex-col gap-1 mb-2">
          <span className="font-semibold text-primary">Category:</span>
          <span>{category}</span>
        </div>
        <div className="flex flex-col gap-1 mb-2">
          <span className="font-semibold text-primary">Account Number:</span>
          <span>{account}</span>
        </div>
      </div>

      <form
        onSubmit={(e) => {
          e.preventDefault();
          if (isValid) onPay(Number(amount));
        }}
        className="flex flex-col gap-4"
      >
        <div>
          <label className="font-semibold text-primary mb-1 block" htmlFor="amount">
            Amount to Pay
          </label>
          <input
            id="amount"
            type="number"
            min="0"
            step="0.01"
            placeholder="Enter amount"
            className="border border-border_default rounded px-4 py-2 w-full"
            value={amount}
            onChange={(e) => setAmount(e.target.value)}
            required
          />
        </div>
        <div className="flex justify-between gap-4">
          <button
            type="submit"
            className="bg-primary hover:bg-comp_hover text-secondary px-6 py-2 rounded font-semibold transition-colors disabled:opacity-50"
            disabled={!isValid}
          >
            Pay
          </button>
          <button
            type="button"
            className="bg-error text-secondary px-6 py-2 rounded font-semibold transition-colors"
            onClick={onCancel}
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  );
};

export default PaymentForm;
