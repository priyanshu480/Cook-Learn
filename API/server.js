import express from "express";
import mongoose from "mongoose";
import userRouter from './routes/user.js'
import bodyParser from 'express'
import recipeRouter from './routes/recipe.js'
import cors from 'cors'


const app = express(); 
app.use(bodyParser.json())
app.use(cors({
  origin:true,
  methods:["GET","POST","PUT","DELETE"],
  credentials:true
}))


app.use('/api',userRouter)
app.use('/api',recipeRouter)



mongoose
  .connect(
    "mongodb+srv://priyanshujha480:HgkCu4DzsUsDzLbd@cluster0.c0wu7ny.mongodb.net/",
    {
      dbName: "food_recipe",
    }
  )
  .then(() => console.log("MongoDB is Connected..!"))
  .catch((err) => console.log(err.message));

const port = 3000;

app.listen(port, () => console.log(`server is running on port ${port}`));