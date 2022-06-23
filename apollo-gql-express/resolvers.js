const Comment = require('./models/Comment.model')

const resolvers = {
    Query: {
        hello: () => {
            return "Hello world!";
        },
        getAllComments: async () => {
            const comments = await Comment.find();
            return comments;
        },

        getCommentById: async (parent, {id}, context, info) => {
            const comment = await Comment.findById(id);
            return comment;
        },

        getCommentsByQuestionId: async (parent, {id}, context, info) => {
            const comments = await Comment.find({ questionId: { $eq: id }});
            return comments;
        }
    },
    Mutation: {
        createComment: async (parent, args, context, info) => {
            const { message, userName, datePosted, questionId} = args.comment;
            const comment = new Comment({ message, userName, datePosted, questionId });
            await comment.save();
            return comment;
        },
        deleteCommentById: async (parent, {id}, context, info) => {
           await Comment.findByIdAndDelete(id);
           return "Comment with id: " + id + " - is deleted"
        },
        updateCommentById: async (parent, args, context, info) => {
            const { message, userName, datePosted, questionId } = args.comment;
            const { id } = args;
            const comment = await Comment.findByIdAndUpdate(id, {message, userName, datePosted, questionId}, {new: true});
            return comment;
        }
    }
};

module.exports = resolvers;