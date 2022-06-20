const express = require('express');


const { graphqlHTTP } = require('express-graphql');

const schema = require('./schema/schema')




const app = express();




//This route will be used as an endpoint to interact with Graphql,

//All queries will go through this route.

app.use('/graphql', graphqlHTTP({

    //directing express-graphql to use this schema to map out the graph

    schema: schema,

    //directing express-graphql to use graphiql when goto '/graphql' address in the browser

    //which provides an interface to make GraphQl queries

    graphiql:true

}));

//MongoDB
const mongoose = require('mongoose');

mongoose.connect('mongodb://localhost:27017/data-store', { useNewUrlParser: true});

mongoose.connection.once('open', () => {
    console.log('connected to database');
});



app.listen(3000, () => {

    console.log('Listening on port 3000');

});