import React, { useState } from 'react';
import { createBorrower } from '../api/Services';

function AddBorrowerForm({ onSuccess }) {
  const [name, setName] = useState('');
  const [phone, setPhone] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    await createBorrower({ borrowerName: name, borrowerPhone: phone });
    onSuccess(); // Return to list
  };

  return (
    <form onSubmit={handleSubmit}>
      <h3>Add a Borrower</h3>
      <input placeholder="Name" value={name} onChange={(e) => setName(e.target.value)} required />
      <input placeholder="Phone" value={phone} onChange={(e) => setPhone(e.target.value)} required />
      <button type="submit">Add</button>
    </form>
  );
}

export default AddBorrowerForm;
