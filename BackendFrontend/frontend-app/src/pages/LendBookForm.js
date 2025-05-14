import React, { useState } from 'react';
import { createBorrowing } from '../api/Services'; // Adjust path as needed

function LendBookForm({ onSuccess }) {
  const [borrowerId, setBorrowerId] = useState('');
  const [itemNo, setItemNo] = useState('');
  const [borrowDate, setBorrowDate] = useState('');
  const [dueDate, setDueDate] = useState('');
  const [branchId, setBranchId] = useState('');
  const [returnStatus, setReturnStatus] = useState(''); // "true" / "false" / ""

  const handleSubmit = async (e) => {
    e.preventDefault();

    const borrowingData = {
      id: Number(itemNo),
      borrowerId: Number(borrowerId),
      borrowDate,
      dueDate,
      branchId: branchId !== '' ? Number(branchId) : 0, // Default to 0
      returnStatus: returnStatus !== '' ? returnStatus === 'true' : false // Default to false
    };

    const success = await createBorrowing(borrowingData);
    if (success) {
      alert('Book lent successfully!');
      onSuccess();
      // Optionally reset form:
      setBorrowerId('');
      setItemNo('');
      setBorrowDate('');
      setDueDate('');
      setBranchId('');
      setReturnStatus('');
    } else {
      alert('Failed to lend the book.');
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <h3>Lend a Book</h3>
      <input
        type="number"
        placeholder="Borrower ID"
        value={borrowerId}
        onChange={(e) => setBorrowerId(e.target.value)}
        required
      />
      <input
        type="number"
        placeholder="ItemNo (Book ID)"
        value={itemNo}
        onChange={(e) => setItemNo(e.target.value)}
        required
      />
      <input
        type="date"
        value={borrowDate}
        onChange={(e) => setBorrowDate(e.target.value)}
        required
      />
      <input
        type="date"
        value={dueDate}
        onChange={(e) => setDueDate(e.target.value)}
        required
      />
      <input
        type="number"
        placeholder="Branch ID (optional)"
        value={branchId}
        onChange={(e) => setBranchId(e.target.value)}
      />
      <select
        value={returnStatus}
        onChange={(e) => setReturnStatus(e.target.value)}
      >
        <option value="">Return Status (optional)</option>
        <option value="true">Returned</option>
        <option value="false">Not Returned</option>
      </select>
      <button type="submit">Lend</button>
    </form>
  );
}

export default LendBookForm;
