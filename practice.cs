s2w6d3

===

q1

===================

recipe.model.ts

export class Recipe{

    id:number;

    name:string;

    type:string;

    ingredients:string[];

    instructions:string;

    constructor(

        id:number,

        name:string,

        type:string,

        ingredients:string[],

        instructions:string

    ){

        this.id=id;

        this.name=name;

        this.type=type;

        this.ingredients=ingredients;

        this.instructions=instructions;

    }

}

============

recipe-list.component.ts

import { Component } from '@angular/core';

//import { Recipe } from '../model/recipe.model';

export interface Recipe{

       id:number,

       name:string,

      type:string,

       ingredients:string[],

     instructions:string

}

@Component({

  selector: 'app-recipe-list',

  templateUrl: './recipe-list.component.html',

  styleUrls: ['./recipe-list.component.css']

})

export class RecipeListComponent {

  recipes: Recipe[]=[

    {

    id:1,

    name:'Pancakes',

    type:'Breakfast',

    ingredients:['Flour','Milk','Eggs','Butter'],

    instructions:'Mix flour,milk and eggs.Cook in a pan with butter.'

    },

    {

      id:2,

      name:'Spaghetti Carbonara',

      type:'Dinner',

      ingredients:['Spaghetti','Eggs','Bacon','Parmesan cheese'],

      instructions:'Mix flour,milk and eggs.Cook in a pan with butter.'

    }

 
  ];

  selectedRecipe: Recipe|null =null;

  showDetails(recipe:Recipe): void{

      this.selectedRecipe=recipe;

  }

  hideDetails(){

    this.selectedRecipe=null;

  }

  deleteRecipe(recipe:Recipe){

     this.recipes=this.recipes.filter(r=>r.id!==recipe.id);

     if(this.selectedRecipe?.id===recipe.id){

      this.selectedRecipe=null;

     }

  }
 
}

=============

recipe-list.component.html
<h1 class="heading">Recipe Manager</h1>
<div class="recipe-list">
<div *ngFor="let recipe of recipes" class="recipe">
<h3>{{recipe.name}}({{recipe.type}})</h3>
<p>{{recipe.ingredients.join(', ')}}</p>
<button (click)="showDetails(recipe)">View Details</button>
</div>
</div>
<div *ngIf="selectedRecipe" class="recipe-details-container">
<h2>Recipe Details</h2>
<p><strong>Name:</strong>{{selectedRecipe.name}}</p>
<p><strong>Type:</strong>{{selectedRecipe.type}}</p>
<p><strong>Ingredients:</strong>{{selectedRecipe.ingredients.join(', ')}}</p>
<p><strong>Instructions:</strong>{{selectedRecipe.instructions}}</p>
<button (click)="hideDetails()">Hide Details</button>
<button (click)="deleteRecipe(selectedRecipe)">Delete</button>
</div>

==============

q2

quiz.component.ts

import { Component } from '@angular/core';
 
import { quizQuestions } from '../../quiz';

@Component({
 
  selector: 'app-quiz',
 
  templateUrl: './quiz.component.html',
 
  styleUrls: ['./quiz.component.css']
 
})
 
export class QuizComponent {
 
  quizQuestions = quizQuestions;
 
  currentQuestionIndex: number = 0;
 
  showFeedback: boolean = false;
 
  feedback: string = '';
 
  score: number = 0;
 
  selectedOptionIndex: number | null = null;
 
  quizEnded: boolean = false;
 
  checkAnswer(optionIndex: number): void {
 
    if (this.showFeedback) {
 
    return;
 
    }
 
    this.selectedOptionIndex = optionIndex;
 
    const currentQuestion = this.quizQuestions[this.currentQuestionIndex];
 
    if (currentQuestion.options[optionIndex]=== currentQuestion.correctAnswer) {
 
    this.feedback = 'Correct Answer!';
 
    this.score++;
 
    } else {
 
    this.feedback = 'Incorrect Answer!';
 
    }
 
    this.showFeedback = true;
 
  }
 
  nextQuestion(): void {
 
    this.currentQuestionIndex++;
 
    this.showFeedback = false;
 
    this.feedback = '';
 
    this.selectedOptionIndex = null;
 
    if (this.currentQuestionIndex >= this.quizQuestions.length) {
 
    this.endQuiz();
 
    }
 
  }
 
  endQuiz(): void {
 
    this.quizEnded = true;
 
  }
 
  restartQuiz(): void {
 
    this.currentQuestionIndex = 0;
 
    this.showFeedback = false;
 
    this.feedback = '';
 
    this.score = 0;
 
    this.selectedOptionIndex = null;
 
    this.quizEnded = false;
 
  }
 
}=======

quiz.component.html
<div class="quiz-container">
 
    <h1>Welcome to the Interactive Quiz Application</h1>
 
    <div *ngIf="!quizEnded">
 
        <h2>
 
            Question {{ currentQuestionIndex + 1 }} of {{ quizQuestions.length }}
 
        </h2>
 
        <p class="question">
 
            {{ quizQuestions[currentQuestionIndex].question }}
 
        </p>
 
        <ul>
<li *ngFor="let option of quizQuestions[currentQuestionIndex].options; let i = index"(click)="checkAnswer(i)" [ngClass]="{  'correct': showFeedback && i === quizQuestions[currentQuestionIndex].correctAnswer,     'incorrect': showFeedback && i === selectedOptionIndex && i !== quizQuestions[currentQuestionIndex].correctAnswer}">
 
                {{ option }}
 
            </li>
 
        </ul>
 
        <p *ngIf="showFeedback" class="feedback">
 
            {{ feedback }}
 
        </p>
 
        <button *ngIf="showFeedback" (click)="nextQuestion()">
 
            Next Question
 
        </button>
 
    </div>
 
    <div *ngIf="quizEnded">
 
        <h2>Quiz Completed!</h2>
 
        <p class="score">
 
            Your Final Score: {{ score }} / {{ quizQuestions.length }}
 
        </p>
 
        <button (click)="restartQuiz()">
 
            Restart Quiz
 
        </button>
 
    </div>
 
</div>
 
