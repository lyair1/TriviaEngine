import React, { Component } from 'react';
import { connect } from 'react-redux';
import * as actions from './actions/actions';
import { Table } from 'react-bootstrap'
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

  getRequestObject() {
    return {
      question: this.state.in_question,
      answer1: this.state.in_answer_1,
      answer2: this.state.in_answer_2,
      answer3: this.state.in_answer_3,
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
          <input disabled={this.props.isFetching} type="text" value={this.state.in_question} placeholder={"Enter Question"} className="App-Input-Question" onChange={(e) => this.setState({in_question: e.target.value})}/>
          
          {/* Answers */}
          <input disabled={this.props.isFetching} type="text" enabled value={this.state.in_answer_1} placeholder={"Answer #1"} className="App-Input-Answer" onChange={(e) => this.setState({in_answer_1: e.target.value})}/>
          <input disabled={this.props.isFetching} type="text" value={this.state.in_answer_2} placeholder={"Answer #2"} className="App-Input-Answer" onChange={(e) => this.setState({in_answer_2: e.target.value})}/>
          <input disabled={this.props.isFetching} type="text" value={this.state.in_answer_3} placeholder={"Answer #3"} className="App-Input-Answer" onChange={(e) => this.setState({in_answer_3: e.target.value})}/>

          {this.props.isFetching ? 
            (<div className="loader"></div>)
            :
            <button disabled={!this.isSubmitEnabled()} className={this.isSubmitEnabled() ? "App-Button" : "App-Button-Disabled"} onClick={() => this.props.getAnswer(this.getRequestObject())}>Find The Answer</button>
        }
          
        </div>

        {this.props.questions.length > 0 &&
          <div className="App-Table-Container">
            <Table striped bordered condensed hover className="App-Table">
              <thead>
                <tr>
                  <th>#</th>
                  <th>Question</th>
                  <th>Answer #1</th>
                  <th>Answer #2</th>
                  <th>Answer #3</th>
                </tr>
              </thead>
              <tbody>
                {
                  this.props.questions.map((question, index) => {
                  let bestIndex = 0;
                  
                  return (
                  <tr key={index}>
                    <td>{index + 1}</td>
                    <td>{question.Question}</td>
                    <td className={bestIndex === 0 ? "App-Table-Correct-td" : ""}>{question.Answers[0].Answer} ({question.Answers[0].Score})</td>
                    <td className={bestIndex === 1 ? "App-Table-Correct-td" : ""}>{question.Answers[1].Answer} ({question.Answers[1].Score})</td>
                    <td className={bestIndex === 2 ? "App-Table-Correct-td" : ""}>{question.Answers[2].Answer} ({question.Answers[2].Score})</td>
                  </tr>
                )})}
              </tbody>
            </Table>
          </div>
        }

        <div>
          <a href="https://github.com/lyair1/TriviaEngine" > <i className="fab fa-github App-Github"></i></a>
        </div>
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    questions: state.questions,
    isFetching: state.isFetching
  };
}

function mapDispatchToProps(dispatch) {
  return {
    getAnswer: ((req) => dispatch(actions.fetchStuff(req)))
  };
}


export default connect(
  mapStateToProps,
  mapDispatchToProps
)(App);
