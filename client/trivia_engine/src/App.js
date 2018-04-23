import React, { Component } from 'react';
import './App.css';

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      in_question: '',
      in_answer_1: '',
      in_answer_2: '',
      in_answer_3: '',
    }
  }

  isSubmitEnabled() {
    return this.state.in_question.length > 0 && this.state.in_answer_1.length > 0 && this.state.in_answer_2.length > 0 && this.state.in_answer_3.length > 0
  }

  render() {
    return (
      <div className="App">
        <h1 className="App-Title">The Trivia Engine</h1>
        <div className="App-Form-Container">
          {/* Questions */}
          <input type="text" value={this.state.in_question} placeholder={"Enter Question"} className="App-Input-Question" onChange={(e) => this.setState({in_question: e.target.value})}/>
          
          {/* Answers */}
          <input type="text" value={this.state.in_answer_1} placeholder={"Answer #1"} className="App-Input-Answer" onChange={(e) => this.setState({in_answer_1: e.target.value})}/>
          <input type="text" value={this.state.in_answer_2} placeholder={"Answer #2"} className="App-Input-Answer" onChange={(e) => this.setState({in_answer_2: e.target.value})}/>
          <input type="text" value={this.state.in_answer_3} placeholder={"Answer #3"} className="App-Input-Answer" onChange={(e) => this.setState({in_answer_3: e.target.value})}/>

          <button disabled={!this.isSubmitEnabled()} className={this.isSubmitEnabled() ? "App-Button" : "App-Button-Disabled"}>Find The Answer</button>
        </div>

        <div>
          <a href="https://github.com/lyair1/TriviaEngine" > <i className="fab fa-github App-Github"></i></a>
        </div>
      </div>
    );
  }
}

export default App;
