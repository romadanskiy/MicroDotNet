const {gql} = require("apollo-server-express");
const typeDefs = gql`
    type Comment {
        id: ID
        message: String
        userName: String
        datePosted: String
        questionId: Int
    }
    
    type Query {
        hello: String
        
        getAllComments: [Comment]
        
        getCommentById(id: ID): Comment
        
        getCommentsByQuestionId(id: ID): [Comment]
    }
    
    input CommentInput {
        message: String
        userName: String
        datePosted: String
        questionId: Int
    }
    
    type Mutation {
        createComment(comment: CommentInput): Comment
        deleteCommentById(id: ID): String
        updateCommentById(id: ID, comment: CommentInput): Comment
    }`;

module.exports = typeDefs;