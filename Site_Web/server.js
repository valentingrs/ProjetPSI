const express = require('express');
const mysql = require('mysql2');
const cors = require('cors');

const app = express();
const port = 3000;

app.use(cors());
app.use(express.json());

const db = mysql.createConnection({
  host: 'localhost',
  user: 'root',
  password: 'root',
  database: 'LivinParis'
});

db.connect(err => {
  if (err) {
    console.error('Erreur de connexion :', err.message);
    return;
  }
  console.log('✅ Connecté à MySQL - LivinParis');
});

app.post('/query', (req, res) => {
  const sql = req.body.query;

  // 🔒 Sécurité basique
  if (/drop\s+database|truncate|shutdown/i.test(sql)) {
    return res.status(403).json({ error: "Commande critique interdite !" });
  }

  db.query(sql, (err, results) => {
    if (err) return res.status(400).json({ error: err.message });
    res.json(results);
  });
});

app.listen(port, () => {
  console.log(`🚀 Serveur opérationnel sur http://localhost:${port}`);
});