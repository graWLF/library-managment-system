import React from 'react';

function BorrowerManagement() {
  return (
    <div>
      <h2>Borrower Management</h2>
      <button>Add Borrower</button>
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Books Borrowed</th>
          </tr>
        </thead>
        <tbody>
          {/* Populate with borrower data */}
        </tbody>
      </table>
    </div>
  );
}

export default BorrowerManagement;