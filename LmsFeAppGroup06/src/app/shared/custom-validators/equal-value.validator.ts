import { AbstractControl, ValidationErrors, ValidatorFn, } from "@angular/forms";

export function equalValueValidator(targetKey: string, toMatchKey: string): ValidatorFn {
    return (formControl: AbstractControl): ValidationErrors | null => {
        const targetControl = formControl.get(targetKey);
        const toMatchControl = formControl.get(toMatchKey);
        if (targetControl?.dirty && toMatchControl?.dirty) {
            if(targetControl.value != toMatchControl.value){
                toMatchControl.setErrors({'equal':`${toMatchControl} is not same as ${targetControl}`})
                return {'equal':`${toMatchControl} is not same as ${targetControl}`};
            }
        }
        return null;
    }
}