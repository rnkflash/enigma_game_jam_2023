using System.Collections.Generic;
using System.IO;
using Ink.Runtime;
using UnityEngine;

public class DialogVariables {

    private static Dictionary<string, Ink.Runtime.Object> variables = null;

    public DialogVariables(TextAsset globalsFile) {
        Story globals = new Story(globalsFile.text);

        if (variables == null) {
            variables = new Dictionary<string, Ink.Runtime.Object>();
            foreach (var name in globals.variablesState)
            {
                Ink.Runtime.Object value = globals.variablesState.GetVariableWithName(name);
                variables.Add(name, value);
            }
        }
    }

    public void StartListening(Story story) {
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story) {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value) {
        if (variables.ContainsKey(name)) {
            variables.Remove(name);
        }

        variables.Add(name, value);
    }

    private void VariablesToStory(Story story) {
        foreach (var variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}