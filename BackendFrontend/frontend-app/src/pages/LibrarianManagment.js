import React, { useEffect, useState } from 'react';
import { fetchLibrarians, createLibrarian, updateLibrarian, deleteLibrarian } from '../api/Services.jsx';

function LibrarianManagment() {
  const [librarians, setLibrarians] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [form, setForm] = useState({ supervisorId: '', librarianName: '' });
  const [editingId, setEditingId] = useState(null);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (editingId) {
        await updateLibrarian(editingId, { ...form, id: editingId });
      } else {
        await createLibrarian({ ...form, id: 0 }); // Set id to 0 by default
      }
      setForm({ supervisorId: '', librarianName: '' });
      setEditingId(null);
      const data = await fetchLibrarians();
      setLibrarians(data);
      setError('');
    } catch (err) {
      console.error('Save librarian error:', err);
      setError('Failed to save librarian: ' + err.message);
    }
  };

  const handleEdit = (lib) => {
    setForm({
      supervisorId: lib.supervisorId,
      librarianName: lib.librarianName,
    });
    setEditingId(lib.id);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this librarian?')) return;
    try {
      await deleteLibrarian(id);
      const data = await fetchLibrarians();
      setLibrarians(data);
      setError('');
    } catch (err) {
      setError('Failed to delete librarian');
    }
  };

  useEffect(() => {
    const loadLibrarians = async () => {
      try {
        const data = await fetchLibrarians();
        setLibrarians(data);
      } catch (err) {
        setError('Failed to fetch librarians');
      } finally {
        setLoading(false);
      }
    };
    loadLibrarians();
  }, []);

  if (loading) return <div>Loading librarians...</div>;
  if (error) return <div style={{ color: 'red' }}>{error}</div>;

  return (
    <div className="librarian-container">
      <h1>Librarian Management</h1>
      <form onSubmit={handleSubmit} style={{ marginBottom: 20 }}>
        <input
          name="supervisorId"
          placeholder="Supervisor"
          value={form.supervisorId}
          onChange={handleChange}
          required
        />
        <input
          name="librarianName"
          placeholder="Name"
          value={form.librarianName}
          onChange={handleChange}
          required
        />
        <button type="submit">{editingId ? 'Update' : 'Add'} Librarian</button>
        {editingId && (
          <button
            type="button"
            onClick={() => {
              setEditingId(null);
              setForm({ supervisorId: '', librarianName: '' });
            }}
          >
            Cancel
          </button>
        )}
      </form>
      <div className="table-scroll">
        <table border="1" cellPadding="8">
          <thead>
            <tr>
              <th>ID</th>
              <th>SPV</th>
              <th>Name</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {librarians.map(lib => (
              <tr key={lib.id}>
                <td>{lib.id}</td>
                <td>{lib.supervisorId}</td>
                <td>{lib.librarianName}</td>
                <td>
                  <button
                    style={{ background: 'none', border: 'none', cursor: 'pointer', marginRight: 8 }}
                    title="Edit"
                    onClick={() => handleEdit(lib)}
                  >
                    ✏️
                  </button>
                  <button
                    style={{ background: 'none', border: 'none', color: 'red', cursor: 'pointer' }}
                    title="Delete"
                    onClick={() => handleDelete(lib.id)}
                  >
                    ❌
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default LibrarianManagment;
