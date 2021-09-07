'use strict';
const fs = require('fs-extra');
const {join} = require('path');

const loadSqlQueries = async (folderName) => {
    const filePath = join(process.cwd(), 'data', folderName);
    const files = await fs.readdir(filePath);
    const sqlFiles = files.filter(f => f.endsWith('.sql'));
    const queries = {};
    for (const sqlfile of sqlFiles) {
        const query = fs.readFileSync(join(filePath,  sqlfile), {encoding: "UTF-8"});
        queries[sqlfile.replace(".sql", "")] = query;
    }
    return queries;
}

module.exports = {
    loadSqlQueries
}

//'use strict';
//
//const fs = require('fs-extra');
//
//const {join} = require('path');
//
//const loadSqlQueries = async (folderName) =>
//{
//    const filePath = join(process.cwd(), 'data', folderName);
//    const Files = await fs.readdir(filePath);
//    const sqlFiles = await Files.filter(f => f.endsWith('.sql'));
//    const queries = {};
//    
//    for(const sqlFile of sqlFiles)
//    {
//        const query = await fs.readFileSync(join(filePath,sqlFile), 
//        {encoding : "UTF-8"});
//        queries[sqlFile.replace(".sql","")] = query;
//    }
//    return queries;
//} 
//
//module.exports =
//{
//    loadSqlQueries
//}