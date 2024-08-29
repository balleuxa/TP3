function calculerAge(dateEntree) {
    var dateNaissance = new Date(dateEntree);
    var aujourdhui = new Date();
    var age = aujourdhui.getFullYear() - dateNaissance.getFullYear();

    if (aujourdhui.getMonth() < dateNaissance.getMonth() ||
        (aujourdhui.getMonth() === dateNaissance.getMonth() && aujourdhui.getDate() < dateNaissance.getDate())) {
        age--;
    }

    return age;
}