#pragma once
#include <corecrt_math.h>
#include "IModel.h"
#include <vector>
#include <list>
#include <set>

class UnitCircleModel : public IModel
{
public:

    UnitCircleModel()
    {
        _angle = NAN;
        _radians = NAN;
        _x = NAN;
        _y = NAN;
    }

    ~UnitCircleModel() { }

    double Angle() const { return _angle; }
    void Angle(double val) { _angle = val; }
    double Radians() const { return _radians; }
    void Radians(double val) { _radians = val; }
    double X() const { return _x; }
    void X(double val) { _x = val; }
    double Y() const { return _y; }
    void Y(double val) { _y = val; }

    void ReadInputs(string csvIn) override;
    void ReadAll(string csvIn) override;
    void WriteAll(string csvOut) override;

private:
    double _angle;
    double _radians;
    double _x;
    double _y;
    set<string> _inputs{ "Angle" };
    set<string> _outputs{ "Radians", "X", "Y" };
    list<UnitCircleModel> _models;
    void UpdateModels(vector<vector<double>>& inputs, set<string> columnSet);
};
